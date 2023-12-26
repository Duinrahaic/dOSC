using Blazor.Diagrams.Core.Geometry;
using dOSCEngine.Engine.Ports;
using dOSCEngine.Engine.Units;
using dOSCEngine.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Concurrent;

namespace dOSCEngine.Engine.Nodes.Utility
{
    public class DelayNode : BaseNode, IDisposable
    {
        public DelayNode(Point? position = null) : base(position ?? new Point(0, 0))
        {
        }
        public DelayNode(Guid guid, Dictionary<string, dynamic>? properties = null, Point? position = null) : base(guid, properties ?? new(), position ?? new Point(0, 0))
        {
            Properties = properties ?? new Dictionary<string, dynamic>();

        }
        protected override void Initialize()
        {
            AddPort(new MultiPort(PortGuids.Port_1, this, true));
            AddPort(new MultiPort(PortGuids.Port_2, this, false));

            // Block Properties
            TryInitializeProperty("DelayTime", (int)100);
            TryInitializeProperty("DelayTimeUnits", TimeUnits.Millisecond);
            TryInitializeProperty("ShowPercent", false);
            TryInitializeProperty("ShowNumbersOnly", false);
            // Parallel Properties

            TryInitializeProperty("MaxQueue", 1);
            TryInitializeProperty("SequentialQueue", true);
            // Standard Properties - Never to be changed or saved
            SetProperty(PropertyType.Name, "Delay");
			SetProperty(PropertyType.Type, GetType().Name.ToString());

            OnPropertyChangeUpdate += DelayNode_OnPropertyChangeUpdate;

            var IsSequential = GetProperty<bool>("SequentialQueue");
        }




        


        private async Task CustomProcessItemAsync(DelayAction item)
        {
            await item.Start();
            await Task.Delay(item.Duration);
            this.Value = item.Value!;
        }


        private void DelayNode_OnPropertyChangeUpdate(string PropertyName, dynamic? Value)
        {
            if (PropertyName.Equals("SequentialQueue",StringComparison.InvariantCultureIgnoreCase))
            {
                Queue.SetSequentialProcessing(Value);
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
                    
                    if(Queue.GetQueueCount() < GetProperty<int>("MaxQueue"))
                    {
                        DelayAction a = new DelayAction(Value, GetDelayTime());
                        Queue.EnqueueItem(a);
                    }
                }
            }

        }

        public int GetQueueCount()
        {
            return Queue.GetQueueCount();
        }
        

        public string GetQueueIndicator()
        {
            if(Queue.HasItemsInQueue())
            {
                var item = Queue.PeekQueue();
                if(item != null)
                {
                    var action = (DelayAction)item;
                    return action.IndicatorToString();
                }
            }
            return "Waiting";
        }
        
       
     
        public string IndicatorToString()
        {
            var currentItem = (DelayAction)Queue.PeekQueue().FirstOrDefault()!;
            if (currentItem != null)
            {
                var RP = currentItem.CalculateRemainingPercent();
                var RT = currentItem.CalculateRemainingTime();

                if(RP == 0)
                {
                    return "Waiting";
                }


                if (GetProperty<bool>("ShowPercent"))
                {
                    return $"{RP.ToString("N1")}%";
                }
                else
                {
                    bool NumbersOnly = GetProperty<bool>("ShowNumbersOnly");
                    return BeautifyString.BeautifyMilliseconds(RT, NumbersOnly);
                }
            }

            return "Waiting";
        }

        public double CalculateRemainingPercent()
        {
            var currentItem = (DelayAction)Queue.PeekQueue().FirstOrDefault()!;

            if (currentItem == null) {
                return 0;
            }
            return currentItem.CalculateRemainingPercent();
        }

        private TimeSpan GetDelayTime()
        {
            long Delay = GetProperty<long>("DelayTime");
            TimeUnits Units = GetProperty<TimeUnits>("DelayTimeUnits");
            TimeSpan ts = TimeSpan.FromMilliseconds(Delay * (long)Units);
            int maxDelayMilliseconds = int.MaxValue;
            if (ts.TotalMilliseconds > maxDelayMilliseconds)
            {
                ts = TimeSpan.FromMilliseconds(maxDelayMilliseconds);
            }
            return ts;
        }

        public void Dispose()
        {
            OnPropertyChangeUpdate -= DelayNode_OnPropertyChangeUpdate;
            Queue.Dispose();
        }
    }
}
