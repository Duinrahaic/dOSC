using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Models.Base;

namespace dOSC.Engine.Ports
{
    public class StringPort : BasePort
    {
        public StringPort(NodeModel parent, bool input, bool limitLink = true) : base(parent, input, limitLink)
        {

        }
        public override bool CanAttachTo(ILinkable other)
        {
            if (!base.CanAttachTo(other)) // default constraints
                return false;
            if (other is not BasePort port)
                return false;
            if(this.Links.Any() && Input && LimitLink)
                return false;
            if (other.Links.Any() && port.Input && LimitLink)
                return false;
            
            if (this.Parent == port.Parent) 
                return false;
            if(Input == port.Input)
                return false;
            if (other is MultiPort multiPort) 
                return multiPort.AllowString;
            if (other is StringPort) 
                return true;
            return false;
        }
    }
}
