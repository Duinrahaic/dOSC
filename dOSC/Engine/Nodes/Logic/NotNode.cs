using Blazor.Diagrams.Core.Anchors;
using Blazor.Diagrams.Core.Models.Base;
using Blazor.Diagrams.Core.Models;
using dOSC.Engine.Ports;
using Blazor.Diagrams.Core.Geometry;

namespace dOSC.Engine.Nodes.Logic
{
    public class NotNode : BaseNode
    {
        public NotNode(Point? position = null) :base(position ?? new Point(0, 0))
        {
            AddPort(new LogicPort(this, false));
            AddPort(new LogicPort(this, true));
        }
        public override string BlockTypeClass => "logicblock";

        public override void Refresh()
        {
            var inA = Ports[0];
            if (inA.Links.Any())
            {
                var l1 = inA.Links.First();
                Value = !Convert.ToBoolean(GetInputValue(inA, l1));
            }
            else
            {
                Value = false;
            }
            base.Refresh();
        }

    }
}
