﻿using Blazor.Diagrams.Core.Models.Base;
using dOSC.Client.Engine.Nodes;

namespace dOSC.Client.Engine.Ports;

public class LogicPort : BasePort
{
    public LogicPort(Guid guid, BaseNode parent, bool input, string name, bool limitLink = true) : base(guid, parent,
        input, name, limitLink, PortType.Logic)
    {
    }

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