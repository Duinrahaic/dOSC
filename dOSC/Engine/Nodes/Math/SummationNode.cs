using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using dOSC.Engine.Ports;

namespace dOSC.Engine.Nodes.Math
{
    public class SummationNode : BaseNode
    {
        public SummationNode(Point? position = null) : base(position ?? new Point(0, 0))
        {
            AddPort(new NumericPort(this, true, false));
            AddPort(new NumericPort(this, false));
        }
        public override string BlockTypeClass => "numericblock";


        public override void Refresh()
        {
            var inputs = Ports[0];

            var sum = 0.0;

            foreach ( var link in inputs.Links )
            {
                var val = GetInputValue(inputs, link);
                if( val != null)
                {
                    sum += val;
                }
            }

            Value = sum;
            base.Refresh();
        }

    }
}
