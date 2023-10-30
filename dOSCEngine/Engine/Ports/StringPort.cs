using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Models.Base;
using dOSCEngine.Engine.Nodes;

namespace dOSCEngine.Engine.Ports
{
    public class StringPort : BasePort
    {
        public StringPort(BaseNode parent, bool input, bool limitLink = true) : base(parent, input, limitLink)
        {

        }
        public StringPort(Guid guid, BaseNode parent, bool input, bool limitLink = true) : base(guid, parent, input, limitLink)
        {

        }
        public override bool CanAttachTo(ILinkable other)
        {
            if (!base.CanAttachTo(other)) // default constraints
                return false;
            if (other is MultiPort multiPort)
                return multiPort.AllowString;
            if (other is StringPort)
                return true;
            return false;
        }
    }
}
