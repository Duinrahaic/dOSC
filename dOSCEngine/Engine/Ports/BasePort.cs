using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Models.Base;
using dOSCEngine.Engine.Nodes;
using Newtonsoft.Json;

namespace dOSCEngine.Engine.Ports
{
    [JsonObject(MemberSerialization.OptIn)]
    public abstract class BasePort : PortModel
    {
        public BasePort(BaseNode parent, bool input, bool limitLink = false) : base(parent,
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
            if (other is not BasePort targetPort) // can only connect to other ports
                return false;
            if (this.Parent == targetPort.Parent) // can't connect to self
                return false;
            if (Input == targetPort.Input) // can't connect input to input or output to output
                return false;
            if(targetPort.Input)
            {
                if (targetPort.Links.Any() && targetPort.LimitLink)
                    return false;
            }
            else
            {
                if (targetPort.Links.Any() && targetPort.LimitLink && this.LimitLink && this.Links.Any())
                    return false;
            }
            return true;
        }
    }
}
