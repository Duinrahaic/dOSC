using Blazor.Diagrams.Core.Geometry;
using dOSCEngine.Engine.Ports;
using dOSCEngine.Engine.Units;
using dOSCEngine.Utilities;
using Newtonsoft.Json;

namespace dOSCEngine.Engine.Nodes.Utility
{
    public class DelayNode : BaseNode
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
            TryInitializeProperty("DelayTime", (int)100);
            TryInitializeProperty("DelayTimeUnits", (int)TimeUnits.millisecond);
            TryInitializeProperty("ShowPercent", false);
            TryInitializeProperty(PropertyType.Name, "Delay");
            TryInitializeProperty(PropertyType.Type, GetType().Name.ToString());
        }
       

        [JsonProperty]
        public override string NodeClass => GetType().Name.ToString();
        public override string BlockTypeClass => "numericblock";
        private bool Delaying = false;
        private DateTime StartTime = DateTime.MinValue;
        private DateTime EndTime = DateTime.MinValue;
        public override void CalculateValue()
        {
            if (Delaying == false)
            {
                var Input = Ports[0];
                if (Input != null)
                {
                    if (Input.Links.Any())
                    {
                        var Value = GetInputValue(Input, Input.Links.First());
                        if (Value != null)
                        {
                            Delaying = true;
                            DelayUpdate(Value);
                        }
                    }
                }
            }
        }

        public string IndicatorToString()
        {
            if(CalculateRemainingPercent() == 0)
            {
                return "Waiting";
            }
            else
            {
                if (GetProperty<bool>("ShowPercent"))
                {
                    return $"{CalculateRemainingPercent().ToString("N1")}%";
                }
                else
                {
                    TimeSpan remainingTime = EndTime - DateTime.Now;
                    return BeautifyString.BeautifyMilliseconds((int)remainingTime.TotalMilliseconds);
                }
            }
            
        }

        public double CalculateRemainingPercent()
        {
            if(Delaying == false)
            {
                return 0;
            }
            TimeSpan totalTime = EndTime - StartTime;
            TimeSpan elapsedTime = DateTime.Now - StartTime;
            TimeSpan remainingTime = EndTime - DateTime.Now;

            // Calculate percentage completion
            double percentRemaining  = ((remainingTime.TotalMilliseconds / totalTime.TotalMilliseconds) * 100);

            return System.Math.Clamp(percentRemaining, 0.0, 100.0);
        }

        private int GetDelayTime()
        {
            int Delay = GetProperty<int>("DelayTime");
            int Units = GetProperty<int>("DelayTimeUnits");
            return System.Math.Clamp(Delay * Units, 1, int.MaxValue);
        }

        private async void DelayUpdate(dynamic? Value)
        {
            StartTime = DateTime.Now;
            EndTime = DateTime.Now.AddMilliseconds(GetDelayTime());
            await Task.Delay(GetDelayTime());
            Delaying = false;
            this.Value = Value!;
        }
    }
}
