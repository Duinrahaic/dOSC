﻿using Blazor.Diagrams.Core.Models.Base;
using Blazor.Diagrams.Core.Models;
using dOSC.Engine.Ports;

namespace dOSC.Engine.Ports
{
    public class MultiPort : BasePort
    {
        public MultiPort(NodeModel parent, bool input, bool limitLink = true, bool allowLogic = true, bool allowNumeric = true, bool allowString = true) : base(parent,input,limitLink)
        {
            AllowLogic = allowLogic;
            AllowNumeric = allowNumeric;
            AllowString = allowString;
        }
        public bool AllowLogic { get; }
        public bool AllowNumeric { get; }
        public bool AllowString { get; }

        private enum PortTypes {
            Numeric,
            Logic,
            String,
            Multi
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
            if (other is MultiPort)
                return true;
            if (other is LogicPort && AllowLogic) 
                return true;
            if (other is NumericPort && AllowNumeric)
                return true;
            if (other is StringPort && AllowString)
                return true;
            return false;
        }
    }
}
