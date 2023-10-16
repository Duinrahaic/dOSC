using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using dOSC.Engine.Ports;
using Newtonsoft.Json;

namespace dOSC.Engine.Nodes.Constant
{
    public class BooleanNode : BaseNode
    {
        public BooleanNode(bool value = false, Point? position = null) : base(position ?? new Point(0, 0))
        {
            AddPort(new LogicPort(PortGuids.Port_1, this, false));
            Value = value;
        }
        public BooleanNode(Guid guid, bool value = false, Point? position = null) : base(guid, position ?? new Point(0, 0))
        {
            AddPort(new LogicPort(PortGuids.Port_1, this, false));
            Value = value;
        }
        [JsonProperty]
        public override string NodeClass => this.GetType().Name.ToString();
        public override string BlockTypeClass => "logicblock";

    }
}
