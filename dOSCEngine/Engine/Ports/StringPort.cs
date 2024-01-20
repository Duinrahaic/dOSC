using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Models.Base;
using dOSCEngine.Engine.Nodes;
using System.Collections.Concurrent;

namespace dOSCEngine.Engine.Ports
{
    public class StringPort : BasePort
    {
 
        public StringPort(Guid guid, BaseNode parent, bool input, string name, bool limitLink = true) : base(guid, parent, input, name, limitLink,PortType.String){}

        public override bool CanAttachTo(ILinkable other)
        {
            if (!base.CanAttachTo(other)) // default constraints
                return false;
            if (other is MultiPort multiPort)
                return multiPort.AllowedTypes.Any(x => x == GetPortType());
            if (other is StringPort)
                return true;
            return false;
        }
    }
}
