using Blazor.Diagrams.Core.Anchors;
using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Models.Base;
using dOSC.Engine.Ports;
using System.Net.NetworkInformation;

namespace dOSC.Engine.Nodes.Logic
{
    public class OrNode : BaseNode
    {
        public OrNode(Point? position = null) : base(position ?? new Point(0, 0))
        {
            AddPort(new LogicPort(this, false));
            AddPort(new LogicPort(this, false));
            AddPort(new LogicPort(this, true));
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
                bool A = Convert.ToBoolean(GetInputValue(inA, l1)); 
                bool B = Convert.ToBoolean(GetInputValue(inB, l2));
                Value = (A || !B) && (!A || B); 
            }
            else
            {
                Value = false;
            }
            base.Refresh();
        }

    }
}
