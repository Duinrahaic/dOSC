using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Models.Base;
using dOSC.Engine.Nodes;
using Newtonsoft.Json;

namespace dOSC.Engine.Ports
{
    [JsonObject(MemberSerialization.OptIn)]
    public abstract class BasePort : PortModel
    {
        public BasePort(BaseNode parent, bool input, bool limitLink = false): base(parent,
            input ? PortAlignment.Left : PortAlignment.Right)
        {
            Input = input;
            ParentGuid = parent.Guid;
            LimitLink = limitLink;
        }
        public BasePort(Guid guid, BaseNode parent, bool input, bool limitLink = false) : base(parent,
            input ? PortAlignment.Left : PortAlignment.Right)
        {
            Guid = guid;
            ParentGuid = parent.Guid;
            Input = input;
            LimitLink = limitLink;
        }
        public Guid Guid { get; set; } = Guid.NewGuid();
        public Guid ParentGuid { get; set; } = Guid.NewGuid();    
        public bool Input { get; }
        public bool LimitLink { get; }
        public override bool CanAttachTo(ILinkable other)
        {
            if (!base.CanAttachTo(other)) // default constraints
                return false;
            return true;
        }
    }
}
