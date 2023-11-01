using Blazor.Diagrams.Core.Geometry;
using dOSCEngine.Engine.Ports;
using dOSCEngine.Utilities;
using Newtonsoft.Json;
using System.Diagnostics;

namespace dOSCEngine.Engine.Nodes.Math
{
    public class SineNode : BaseNode, IDisposable
    {
        public SineNode(Point? position = null) : base(position ?? new Point(0, 0))
        {
            AddPort(new NumericPort(PortGuids.Port_1, this, true));
            AddPort(new NumericPort(PortGuids.Port_2, this, true));
            AddPort(new NumericPort(PortGuids.Port_3, this, false));
            GlobalTimer.OnTimerElapsed += GetSineWave;
        }
        public SineNode(Guid guid, Point? position = null) : base(guid, position ?? new Point(0, 0))
        {
            AddPort(new NumericPort(PortGuids.Port_1, this, true));
            AddPort(new NumericPort(PortGuids.Port_2, this, true));
            AddPort(new NumericPort(PortGuids.Port_3, this, false));
            GlobalTimer.OnTimerElapsed += GetSineWave;
        }
        [JsonProperty]
        public override string NodeClass => GetType().Name.ToString();
        public override string BlockTypeClass => "numericblock";

        private static double _amplitude = 1;
        private static double _frequency = 1;

        private void GetSineWave()
        {
            var i1 = Ports[0]; // amplitude
            var i2 = Ports[1]; // frequency
            if (i1.Links.Count > 0)
            {
                var l1 = i1.Links[0];
                var v1 = GetInputValue(i1, l1);
                if (v1 == null)
                {
                    _amplitude = 1;
                }
                else
                {
                    _amplitude = v1;
                }
            }
            if (i2.Links.Count > 0)
            {
                var l2 = i2.Links[0];
                var v2 = GetInputValue(i2, l2);
                if (v2 == null)
                {
                    _frequency = 1;
                }
                else
                {
                    _frequency = v2;
                }
            }

            double time = DateTime.Now.TimeOfDay.TotalSeconds; // Current time in seconds
            Value = _amplitude * System.Math.Sin(2 * System.Math.PI * _frequency * time);
        }

        public void Dispose()
        {
            GlobalTimer.OnTimerElapsed -= GetSineWave;
        }
    }
}
