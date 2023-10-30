using Blazor.Diagrams.Core.Models.Base;
using Blazor.Diagrams.Core.Models;
using dOSCEngine.Engine.Nodes;

namespace dOSCEngine.Engine.Ports
{
    public class MultiPort : BasePort
    {
        public MultiPort(BaseNode parent, bool input, bool limitLink = true, bool allowLogic = true, bool allowNumeric = true, bool allowString = true) : base(parent, input, limitLink)
        {
            AllowLogic = allowLogic;
            AllowNumeric = allowNumeric;
            AllowString = allowString;
        }
        public MultiPort(Guid guid, BaseNode parent, bool input, bool limitLink = true, bool allowLogic = true, bool allowNumeric = true, bool allowString = true) : base(guid, parent, input, limitLink)
        {
            AllowLogic = allowLogic;
            AllowNumeric = allowNumeric;
            AllowString = allowString;
        }
        public bool AllowLogic { get; }
        public bool AllowNumeric { get; }
        public bool AllowString { get; }

        private enum PortTypes
        {
            Numeric,
            Logic,
            String,
            Multi
        }

        public override bool CanAttachTo(ILinkable other)
        {
            if (!base.CanAttachTo(other)) // default constraints
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
