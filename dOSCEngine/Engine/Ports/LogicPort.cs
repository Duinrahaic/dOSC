using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Models.Base;
using dOSCEngine.Engine.Nodes;
using System.Collections.Concurrent;
using System.Xml.Linq;

namespace dOSCEngine.Engine.Ports
{
    public class LogicPort : BasePort
    {


        public LogicPort(Guid guid, BaseNode parent, bool input, string name, bool limitLink = true) : base(guid, parent, input, name, limitLink,PortType.Logic){}

        public override bool CanAttachTo(ILinkable other)
        {
            if (!base.CanAttachTo(other)) // default constraints
                return false;
            if (other is MultiPort multiPort)
                return multiPort.AllowedTypes.Any(x => x == GetPortType());
            if (other is LogicPort)
                return true;
            return false;
        }
    }
}
