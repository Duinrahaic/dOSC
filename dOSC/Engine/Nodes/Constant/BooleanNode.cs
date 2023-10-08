using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using dOSC.Engine.Ports;

namespace dOSC.Engine.Nodes.Constant
{
    public class BooleanNode : BaseNode
    {
        public BooleanNode(bool value = false, Point? position = null) : base(position ?? new Point(0, 0))
        {
            AddPort(new LogicPort(this, false));
            Value = value;
        }
        public override string BlockTypeClass => "logicblock";

    }
}
