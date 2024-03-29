﻿using Blazor.Diagrams.Core.Models.Base;
using Blazor.Diagrams.Core.Models;
using dOSCEngine.Engine.Nodes;
using System.Xml.Linq;
using System.Collections.Concurrent;

namespace dOSCEngine.Engine.Ports
{
    public class NumericPort : BasePort
    {
 
        public NumericPort(Guid guid, BaseNode parent, bool input, string name, bool limitLink = true) : base(guid, parent, input, name, limitLink,PortType.Numeric){}

        public override bool CanAttachTo(ILinkable other)
        {
            if (!base.CanAttachTo(other)) // default constraints
                return false;
            if (other is MultiPort multiPort)
                return multiPort.AllowedTypes.Any(x => x == GetPortType());
            if (other is NumericPort)
                return true;
            return false;
        }
    }
}
