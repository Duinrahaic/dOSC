using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Models.Base;
using dOSC.Engine.Nodes;

namespace dOSC.Engine.Ports
{
    public class LogicPort : BasePort
    {
        public LogicPort(BaseNode parent, bool input, bool limitLink = true) : base(parent,input,limitLink)
        {

        }
        public LogicPort(Guid guid, BaseNode parent, bool input, bool limitLink = true) : base(guid, parent, input, limitLink)
        {

        }
        public override bool CanAttachTo(ILinkable other)
        {
            if (!base.CanAttachTo(other)) // default constraints
                return false;
            if (other is not BasePort port)
                return false;
            if (this.Links.Any() && Input && LimitLink)
                return false;
            if (other.Links.Any() && port.Input && LimitLink)
                return false;

            if (this.Parent == port.Parent)
                return false;
            if (Input == port.Input)
                return false;
            if (other is MultiPort multiPort)
                return multiPort.AllowLogic;
            if (other is LogicPort)
                return true;
            return false;
        }
    }
}
