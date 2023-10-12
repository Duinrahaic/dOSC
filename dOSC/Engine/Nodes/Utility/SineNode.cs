using Blazor.Diagrams.Core.Geometry;
using dOSC.Engine.Ports;
using dOSC.Utilities;
using System.Diagnostics;

namespace dOSC.Engine.Nodes.Utility
{
    public class SineNode : BaseNode, IDisposable
    {
        public SineNode(Point? position = null) : base(position ?? new Point(0, 0))
        {
            AddPort(new NumericPort(this, true));
            AddPort(new NumericPort(this, true));
            AddPort(new NumericPort(this, false));
            GlobalTimer.OnTimerElapsed += GetSineWave;
        }
        public override string BlockTypeClass => "numericblock";

        private static double _amplitude = 1;
        private static double _frequency = 1;
        public override void Refresh()
        {
            base.Refresh();
        }

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
