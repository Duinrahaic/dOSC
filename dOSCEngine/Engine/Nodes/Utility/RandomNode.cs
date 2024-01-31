using Blazor.Diagrams.Core;
using Blazor.Diagrams.Core.Geometry;
using dOSCEngine.Engine.Ports;
using dOSCEngine.Engine.Units;
using dOSCEngine.Utilities;
using Newtonsoft.Json;
using System.Collections.Concurrent;
using static dOSCEngine.Engine.Nodes.Utility.DelayNode;
using System.Diagnostics;

namespace dOSCEngine.Engine.Nodes.Utility
{
    public class RandomNode : BaseNode
    {
        public RandomNode(Guid? guid = null, ConcurrentDictionary<EntityProperty, dynamic>? properties = null, Point? position = null) : base(guid, position, properties)
        {
            AddPort(new MultiPort(PortGuids.Port_1, this, false, allowedTypes: new() { PortType.Numeric, PortType.Logic, PortType.Multi }, name: "Output"));

            Properties.TryInitializeProperty(EntityProperty.DelayTime, (int)1);
            Properties.TryInitializeProperty(EntityProperty.DelayTimeUnits, TimeUnits.Second);
            Properties.TryInitializeProperty(EntityProperty.Max, (double) 255.0);
            Properties.TryInitializeProperty(EntityProperty.Min,(double) -255.0);
            Properties.TryInitializeProperty(EntityProperty.DecimalPlaceCount, 0);
            Properties.TryInitializeProperty(EntityProperty.ShowPercent, false);
            Properties.TryInitializeProperty(EntityProperty.ShowNumbersOnly, false);
            ShowProgressBar = true;
            VisualIndicator = IndicatorToString();
            _delayTime = Properties.GetProperty<long>(EntityProperty.DelayTime);
            _delayTimeUnits = Properties.GetProperty<TimeUnits>(EntityProperty.DelayTimeUnits);
            _showPercent = Properties.GetProperty<bool>(EntityProperty.ShowPercent);
            _showNumbersOnly = Properties.GetProperty<bool>(EntityProperty.ShowNumbersOnly);
            _max = Properties.GetProperty<double>(EntityProperty.Max);
            _min = Properties.GetProperty<double>(EntityProperty.Min);
            _decimalPlaces = Properties.GetProperty<int>(EntityProperty.DecimalPlaceCount);
            
            Queue = new QueueProcessor<object>(
                customProcessItemAsync: Update,
                isSequential: true);
            Queue.StartProcessing();
            GlobalTimer.OnTimerElapsed += QueueTask;

            SubscribeToAllPortTypeChanges();
        }
        
        public override string Name => "Random";
        public override string Category => NodeCategoryType.Utilities;
        public override string Icon => "icon-dices";
        
        
        
        private static readonly Random Random = new Random();
        private QueueProcessor<object> Queue;
        private long _delayTime;
        private TimeUnits _delayTimeUnits;
        private bool _showPercent;
        private bool _showNumbersOnly;
        private DateTime _endTime = DateTime.MinValue;
        private double _max;
        private double _min;
        private int _decimalPlaces;

        private object QueueLock = new object();
        
        public override void PropertyNotifyEvent(EntityProperty property, dynamic? value)
        {
            
            if(property == EntityProperty.DelayTime)
            {
                _delayTime = value;
            }
            else if(property == EntityProperty.DelayTimeUnits)
            {
                _delayTimeUnits = value;
            }
            else if(property == EntityProperty.ShowPercent)
            {
                _showPercent = value;
            }
            else if(property == EntityProperty.ShowNumbersOnly)
            {
                _showNumbersOnly = value;
            }
            else if(property == EntityProperty.Max)
            {
                _max = value;
            }
            else if(property == EntityProperty.Min)
            {
                _min = value;
            }
            else if(property == EntityProperty.DecimalPlaceCount)
            {
                _decimalPlaces = value;
            }
        }
        
        private void QueueTask()
        {
            lock (QueueLock)
            {
                if (CalculateRemainingTime() == TimeSpan.Zero && GetAllPorts().Any(x=>x.HasValidLinks()))
                {
                    Queue.EnqueueItem(new());
                }
            }
        }
 

        private async Task Update(object _)
        {
            _endTime = DateTime.Now.AddMilliseconds(GetDelayTime().TotalMilliseconds);
            do
            {
                Progress = CalculateRemainingPercent();
                VisualIndicator = IndicatorToString();
                await Task.Delay(100);
            } while (CalculateRemainingPercent() != 0);
            VisualIndicator = IndicatorToString();   
            Progress = CalculateRemainingPercent();
            
            dynamic result = null!;
            PortType? pt = GetCurrentMultiPortType();
            if (pt.HasValue)
            {
                if (pt == PortType.Numeric)
                {

                    result = System.Math.Round(GetRandomNumber(_min, _max), _decimalPlaces);
                }
                else if (pt == PortType.Logic)
                {
                    result = GetRandomBool();
                }

                if(pt != PortType.Multi)
                {
                    Value = result;
                }
            }
        }
        private double GetRandomNumber(double minimum, double maximum)
        {
            return Random.NextDouble() * (maximum - minimum) + minimum;
        }

        private bool GetRandomBool()
        {
            return Random.NextInt64() % 2 == 0;
        }
        public override void CalculateValue()
        {
            // Do nothing here
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
        public string IndicatorToString()
        {
            var RP = CalculateRemainingPercent();
            var RT = CalculateRemainingTime();

            if (GetDelayTime() == TimeSpan.Zero)
            {
                return "Instant";
            }

            if (RP == 0)
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

        

        public double CalculateRemainingPercent()
        {
            if (GetDelayTime() == TimeSpan.Zero)
                return 0;

            TimeSpan remainingTime = CalculateRemainingTime();
            // Calculate percentage completion
            double percentRemaining = ((remainingTime.TotalMilliseconds / GetDelayTime().TotalMilliseconds) * 100);
            return System.Math.Clamp(percentRemaining, 0.0, 100.0);
        }
        public TimeSpan CalculateRemainingTime()
        {
            TimeSpan remainingTime = this._endTime - DateTime.Now;
            if (remainingTime < TimeSpan.Zero)
                return TimeSpan.Zero;
            return remainingTime;
        }
        
        
        public override void OnDispose()
        {
            UnsubscribeToAllPortTypeChanged();
            Queue.Dispose();
            GlobalTimer.OnTimerElapsed -= QueueTask;
            base.OnDispose();
        }
    }
}
