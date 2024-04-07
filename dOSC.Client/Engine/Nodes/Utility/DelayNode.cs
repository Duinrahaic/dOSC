using System.Collections.Concurrent;
using Blazor.Diagrams.Core.Geometry;
using dOSC.Client.Engine.Ports;
using dOSC.Shared.Models.Wiresheet;
using dOSC.Shared.Units;
using dOSC.Shared.Utilities;

namespace dOSC.Client.Engine.Nodes.Utility
{
    public class DelayNode : BaseNode
    {
        public DelayNode(Guid? guid = null, ConcurrentDictionary<EntityPropertyEnum, dynamic>? properties = null, Point? position = null) : base(guid, position, properties)
        {
            AddPort(new MultiPort(PortGuids.Port_1, this, input: true, "Value"));
            AddPort(new MultiPort(PortGuids.Port_2, this, input: false, "Output"));

            // Block Properties
            Properties.TryInitializeProperty(EntityPropertyEnum.DelayTime, (int)1);
            Properties.TryInitializeProperty(EntityPropertyEnum.DelayTimeUnits, TimeUnit.Second);
            Properties.TryInitializeProperty(EntityPropertyEnum.MaxQueue, 1);
            Properties.TryInitializeProperty(EntityPropertyEnum.DecimalPlaceCount, 0);
            Properties.TryInitializeProperty(EntityPropertyEnum.ShowPercent, false);
            Properties.TryInitializeProperty(EntityPropertyEnum.ShowNumbersOnly, false);
            
            _delayTime = Properties.GetProperty<long>(EntityPropertyEnum.DelayTime);
            _delayTimeUnits = Properties.GetProperty<TimeUnit>(EntityPropertyEnum.DelayTimeUnits);
            _showPercent = Properties.GetProperty<bool>(EntityPropertyEnum.ShowPercent);
            _showNumbersOnly = Properties.GetProperty<bool>(EntityPropertyEnum.ShowNumbersOnly);
            QueueSize = Properties.GetProperty<int>(EntityPropertyEnum.MaxQueue);
            ShowProgressBar = true;
            VisualIndicator = IndicatorToString();
            
            Queue = new QueueProcessor<DelayAction>(
                customProcessItemAsync: Process,
                isSequential: true);
            Queue.StartProcessing();
            SubscribeToAllPortTypeChanges();
        }

        public override string Name => "Delay";
        public override string Category => NodeCategoryType.Utilities;
        public override string Icon => "icon-clock";
        
        
        
        public override void PropertyNotifyEvent(EntityPropertyEnum property, dynamic? value)
        {
            if(property == EntityPropertyEnum.DelayTime)
            {
                _delayTime = value;
            }
            else if(property == EntityPropertyEnum.DelayTimeUnits)
            {
                _delayTimeUnits = value;
            }
            else if(property == EntityPropertyEnum.ShowPercent)
            {
                _showPercent = value;
            }
            else if(property == EntityPropertyEnum.ShowNumbersOnly)
            {
                _showNumbersOnly = value;
            }
            else if(property == EntityPropertyEnum.MaxQueue)
            {
                QueueSize = (int)value;
            }

        }
        


        private QueueProcessor<DelayAction> Queue = new QueueProcessor<DelayAction>();
        private DelayAction? ActiveAction;
        private long _delayTime = 1;
        private TimeUnit _delayTimeUnits = TimeUnit.Millisecond;
        private bool _showPercent = false;
        private bool _showNumbersOnly = false;

        private async Task Process(DelayAction item)
        {
            ActiveAction = item;
            await ActiveAction.Start();
            do
            {
                Progress = ActiveAction.CalculateRemainingPercent();
                VisualIndicator = ActiveAction.IndicatorToString(Percent:_showPercent, NumbersOnly:_showNumbersOnly);
                if (QueueSize > 1)
                {
                    ItemsInQueue = Queue.GetQueueCount();
                }
                
                

                
                await Task.Delay(100);
            } while (ActiveAction.CalculateRemainingPercent() != 0);
            
            VisualIndicator = ActiveAction.IndicatorToString();   
            Progress = ActiveAction.CalculateRemainingPercent();
            
            
            if(GetCurrentMultiPortType() == PortType.Multi)
            {
                SetValue(null!, false);
                Queue.ClearQueue();
            }
            else
            {
                this.Value = ActiveAction.Value!;
            }
        }

 
        public class DelayAction
        {
            public string Guid = System.Guid.NewGuid().ToString();
            public dynamic? Value { get; set; }
            public DateTime StartTime { get; set; } 
            public DateTime EndTime { get; set; }
            public TimeSpan Duration { get; set; }

            public DelayAction(dynamic? Value, TimeSpan Duration)
            {
                this.Value = Value;
                this.Duration = Duration;
            }
            
            public async Task Start()
            {
                this.StartTime = DateTime.Now;
                this.EndTime = DateTime.Now.AddMilliseconds(Duration.TotalMilliseconds);
                await Task.CompletedTask;
            }

            public string IndicatorToString(bool Percent = false, bool NumbersOnly = false)
            {
                if (CalculateRemainingPercent() == 0)
                {
                    return "Waiting";
                }
                else
                {
                    if (Percent)
                    {
                        return $"{CalculateRemainingPercent().ToString("N1")}%";
                    }
                    else
                    {
                        TimeSpan remainingTime = this.EndTime - DateTime.Now;
                        return BeautifyString.BeautifyMilliseconds(remainingTime, NumbersOnly);
                    }
                }
            }

            public double CalculateRemainingPercent()
            {
                TimeSpan remainingTime = CalculateRemainingTime();
                // Calculate percentage completion
                double percentRemaining = ((remainingTime.TotalMilliseconds / Duration.TotalMilliseconds) * 100);
                return System.Math.Clamp(percentRemaining, 0.0, 100.0);
            }

            public TimeSpan CalculateRemainingTime()
            {
                TimeSpan remainingTime = this.EndTime - DateTime.Now;
                if (remainingTime < TimeSpan.Zero)
                    return TimeSpan.Zero;
                return remainingTime;
            }
        }

        public override void CalculateValue()
        {
            var Input = Ports[0];
            if (Input != null)
            {
                if (Input.Links.Any())
                {
                    var Value = GetInputValue(Input, Input.Links.First());

                    if (Queue.GetQueueCount() < QueueSize)
                    {
                        DelayAction a = new DelayAction(Value, GetDelayTime());
                        Queue.EnqueueItem(a);
                    }
                }
            }

        }

        public int GetCountInQueue()
        {
            return Queue.GetQueueCount();
        }
        
        public string GetQueueIndicator()
        {
            if (Queue.HasItemsInQueue())
            {
                if (ActiveAction != null)
                {
                    return ActiveAction.IndicatorToString();
                }
            }
            return "Waiting";
        } 
        
        

        public string IndicatorToString()
        {
            if (ActiveAction != null)
            {
                var RP = ActiveAction.CalculateRemainingPercent();
                var RT = ActiveAction.CalculateRemainingTime();

                if(ActiveAction.Duration == TimeSpan.Zero)
                {
                    return "Instant";
                }

                if(RP == 0)
                {
                    return "Waiting";
                }


                if (_showPercent)
                {
                    return $"{RP.ToString("N1")}%";
                }
                else
                {
                    return BeautifyString.BeautifyMilliseconds(RT, _showNumbersOnly);
                }
            }

            return "Waiting";
        }

        public double CalculateRemainingPercent()
        {
            if (ActiveAction == null) {
                return 0;
            }

            if (ActiveAction.Duration == TimeSpan.Zero)
            {
                return 0;
            }

            return ActiveAction.CalculateRemainingPercent();
        }



      
        
        private TimeSpan GetDelayTime()
        {
            TimeSpan ts = TimeSpan.FromMilliseconds(_delayTime * (long)_delayTimeUnits);
            int maxDelayMilliseconds = int.MaxValue;
            if (ts.TotalMilliseconds > maxDelayMilliseconds)
            {
                ts = TimeSpan.FromMilliseconds(maxDelayMilliseconds);
            }
            return ts;
        }

        public override void OnDispose()
        {
            UnsubscribeToAllPortTypeChanged();
            Queue.Dispose();
        }
    }
}
