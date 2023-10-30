using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Models.Base;
using dOSCEngine.Engine.Nodes;

namespace dOSCEngine.Engine.Ports
{
    public class LogicPort : BasePort
    {
        public LogicPort(BaseNode parent, bool input, bool limitLink = true) : base(parent, input, limitLink)
        {

        }
        public LogicPort(Guid guid, BaseNode parent, bool input, bool limitLink = true) : base(guid, parent, input, limitLink)
        {

        }
        public override bool CanAttachTo(ILinkable other)
        {
            if (!base.CanAttachTo(other)) // default constraints
                return false;
            if (other is MultiPort multiPort)
                return multiPort.AllowLogic;
            if (other is LogicPort)
                return true;
            return false;
        }
    }
}
