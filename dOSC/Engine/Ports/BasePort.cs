using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Models.Base;

namespace dOSC.Engine.Ports
{
    public abstract class BasePort : PortModel
    {
        public BasePort(NodeModel parent, bool input, bool limitLink = false): base(parent,
            input ? PortAlignment.Left : PortAlignment.Right)
        {
            Input = input;
            LimitLink = limitLink;
        }
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
