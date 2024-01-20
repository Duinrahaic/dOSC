using Blazor.Diagrams.Core.Anchors;
using Blazor.Diagrams.Core.Models.Base;
using dOSCEngine.Engine.Nodes;
using System.Collections.Concurrent;

namespace dOSCEngine.Engine.Ports
{
    public class MultiPort : BasePort
    {
        public MultiPort(Guid guid, BaseNode parent, bool input, string name, bool limitLink = true, List<PortType>? allowedTypes = null) : base(guid, parent, input, name, limitLink, PortType.Multi)
        {
            if (allowedTypes == null)
            {
                allowedTypes = Enum.GetValues(typeof(PortType)).Cast<PortType>().ToList();
            }
            AllowedTypes = allowedTypes;
            this.OnPortLinksChanged += OnLinksChanged;
         
        }
        public List<PortType> AllowedTypes { get; private set; }
        
        private void OnLinksChanged(Model obj)
        {

            var firstBaseLink = GetFirstBaseLink();
            if (firstBaseLink != null)
            {
                PortType newPortType = this.GetPortTypeOverride();

                if (newPortType == PortType.Multi)
                {
                    var firstLink = Links.FirstOrDefault();
                    if (firstLink != null)
                    {
                        var sp = (firstBaseLink.Source as SinglePortAnchor)!;
                        var tp = (firstBaseLink.Target as SinglePortAnchor)!;
                        if (sp.Port is BasePort spn && tp.Port is BasePort tpn)
                        {
                            List<PortType> lst = new List<PortType>();

                            lst.Add(spn.GetPortType());
                            lst.Add(tpn.GetPortType());
                            var t = lst.Max();
                            newPortType = t;
                        }
                    }
                }
                SetPortTypeOverride(newPortType);
            }
            else
            {
                ResetPortTypeOverride();
            }

            
        }
        
        public override bool CanAttachTo(ILinkable other)
        {
           
            if (!base.CanAttachTo(other)) // default constraints
                return false;
            if(other is MultiPort) 
                return true;
            if (other is LogicPort lp &&AllowedTypes.Any(x=>x == PortType.Logic))
            {
                return lp.GetPortType() == GetPortTypeOverride() || GetPortTypeOverride() == PortType.Multi;
            }
            if (other is NumericPort np && AllowedTypes.Any(x => x == PortType.Numeric))
            {
                return np.GetPortType() == GetPortTypeOverride() || GetPortTypeOverride() == PortType.Multi;
            }
            if (other is StringPort sp && AllowedTypes.Any(x => x == PortType.String))
            {
                return sp.GetPortType() == GetPortTypeOverride() || GetPortTypeOverride() == PortType.Multi;
            }
            return false;
        }

        public override void OnDispose()
        {
        }
    }
}
