using Blazor.Diagrams.Core.Geometry;
using dOSC.Engine.Ports;

namespace dOSC.Engine.Nodes.Logic
{
    public class GreaterThenNode : BaseNode
    {
        public GreaterThenNode(Point? position = null) : base(position ?? new Point(0, 0))
        {
            AddPort(new NumericPort(this, true));
            AddPort(new NumericPort(this, true));
            AddPort(new LogicPort(this, false));
        }
        public override string BlockTypeClass => "logicblock";

        public override void Refresh()
        {
            var inA = Ports[0];
            var inB = Ports[1];
            if (inA.Links.Any() && inB.Links.Any())
            {
                var l1 = inA.Links.First();
                var l2 = inB.Links.First();

                var valA = GetInputValue(inA, l1);
                var valB = GetInputValue(inB, l2);
                Value = valA > valB;
            }
            else
            {
                Value = false;
            }
            base.Refresh();
        }


    }
}
