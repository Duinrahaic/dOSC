using Blazor.Diagrams.Core.Geometry;
using dOSC.Engine.Ports;

namespace dOSC.Engine.Nodes.Constant
{
    public class NumericNode : BaseNode
    {
        public NumericNode(double value = 1.0, Point? position = null) : base(position ?? new Point(0, 0))
        {
            AddPort(new NumericPort(this, false));
            Value = value;
        }
        public override string BlockTypeClass => "numericblock";

    }
}
