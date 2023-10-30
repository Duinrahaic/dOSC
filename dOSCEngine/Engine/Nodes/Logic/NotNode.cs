using Blazor.Diagrams.Core.Anchors;
using Blazor.Diagrams.Core.Models.Base;
using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Geometry;
using Newtonsoft.Json;
using dOSCEngine.Engine.Nodes;
using dOSCEngine.Engine.Ports;

namespace dOSCEngine.Engine.Nodes.Logic
{
    public class NotNode : BaseNode
    {
        public NotNode(Point? position = null) : base(position ?? new Point(0, 0))
        {
            AddPort(new LogicPort(PortGuids.Port_1, this, false));
            AddPort(new LogicPort(PortGuids.Port_2, this, true));
        }
        public NotNode(Guid guid, Point? position = null) : base(guid, position ?? new Point(0, 0))
        {
            AddPort(new LogicPort(PortGuids.Port_1, this, false));
            AddPort(new LogicPort(PortGuids.Port_2, this, true));
        }
        [JsonProperty]
        public override string NodeClass => GetType().Name.ToString();
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
