using Blazor.Diagrams.Core.Geometry;
using dOSC.Engine.Ports;

namespace dOSC.Engine.Nodes.Utility
{
    public class MinNode : BaseNode
    {
        public MinNode(Point? position = null) : base(position ?? new Point(0, 0))
        {
            AddPort(new NumericPort(this, true, false));
            AddPort(new NumericPort(this, false));
        }
        public override string BlockTypeClass => "numericblock";


        public override void Refresh()
        {
            var inputs = Ports[0];
            if (!inputs.Links.Any())
            {
                Value = null;
            }
            else
            {
                List<double> values = new List<double>();
                inputs.Links.Select(x => GetInputValue(inputs, x)).Where(x => x != null).Select(x => values.Add(x));
                if (values.Any())
                {
                    Value = values.Min();
                }
                else
                {
                    Value = null;
                }
            }
            base.Refresh();
        }

    }
}
