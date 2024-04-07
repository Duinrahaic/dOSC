using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Models.Base;
using dOSC.Client.Engine.Links;
using dOSC.Client.Engine.Nodes;


namespace dOSC.Client.Engine.Ports
{
    public abstract class BasePort : PortModel, IDisposable
    {
        public delegate void PortLinksChanged(BasePort port);
        public event PortLinksChanged? OnPortLinksChanged;
        public delegate void PortTypeChanged(Guid portGuid, PortType portType);
        public event PortTypeChanged? OnPortTypeChanged;



        protected BasePort(Guid guid,BaseNode parent, bool input, string name, bool limitLink = false, PortType type = PortType.None) : base(parent,
            input ? PortAlignment.Left : PortAlignment.Right)
        {

            Guid = guid;
            ParentGuid = parent.Guid;
            Input = input;
            Name = name;
            LimitLink = limitLink;
            PortType = type;
            _portTypeOverride = type;
            this.Changed += PortUpdated;
            var autoEvent = new AutoResetEvent(true);
            _portWatcher = new(UpdateLinkCount,autoEvent, 100,100);
        }
        
        

        public virtual void PortUpdated(Model obj)
        {
            if (LimitLink && Input)
            {
                if (Links.Any())
                {
                    AtMaxInputs = true;
                }
                else
                {
                    AtMaxInputs = false;
                }
            }
            else
            {
                AtMaxInputs = false;
            }
        }
        
        #region Common properties
        public Guid Guid { get; set; }
        public Guid ParentGuid { get; set; }
        public bool Input { get; } = false;
        public bool LimitLink { get; } = false;
        public String Name { get; set; }
    
        private Timer _portWatcher;

        private bool _atMaxInputs = false;
        public bool AtMaxInputs
        {
            get => _atMaxInputs;
            set
            {
                if (value)
                {
                    _atMaxInputs = true;
                    Locked = true;
                }
                else
                {
                    _atMaxInputs = false;
                    Locked = false;
                }
            }
        }
        
        
        
        public bool HasError { get; set; }
        #endregion

        #region  Link Logic
        public List<BaseLink> GetAllBaseLinks()
        {
            List<BaseLink> CurrentLinks = new();
            Links.ToList().ForEach(link =>
            {
                var PortTarget = link.Target.Model as BasePort;
                var PortSource = link.Source.Model as BasePort;

                if (PortTarget != null && PortSource != null)
                {
                    CurrentLinks.Add(new BaseLink(PortSource,PortTarget));
                }
            });
            return CurrentLinks ?? new();   
        }

        protected BaseLink? GetFirstBaseLink()
        {
            var tl = Links.ToList().FirstOrDefault();
            if (tl != null)
            {
                var portTarget = tl.Target.Model as BasePort;
                var portSource = tl.Source.Model as BasePort;

                if (portTarget != null && portSource != null)
                {
                    return new BaseLink(portSource,portTarget);
                }
            }
            return null;
        }
    
        
        

        private int _validLinkCount = 0;
        private int _previousLinkCount = 0;
        private int ValidLinkCount
        {
            get => _validLinkCount;
            set
            {
                _validLinkCount = value;
                if(_previousLinkCount != _validLinkCount)
                {
                    _previousLinkCount = _validLinkCount;
                    OnPortLinksChanged?.Invoke(this);
                }
            }
        }
        private void UpdateLinkCount(object? _)
        {
            ValidLinkCount = GetAllLinks().Count;
        }
        public void UpdateLinkCount()
        {
            ValidLinkCount = GetAllLinks().Count;
        }
        public bool HasValidLinks()
        {
            return GetAllLinks().Any();
        }
        private bool _isExecuting = false;

        private bool DoesLinkExist(BasePort a, BasePort b)
        {
            List<(BasePort Source, BasePort Target)> portALinks = a.GetAllLinks();
            List<(BasePort Source, BasePort Target)> portBLinks = b.GetAllLinks();


            var listALinkFilter = portALinks.Where(x => (x.Target.ParentGuid == a.ParentGuid || x.Source.ParentGuid == a.ParentGuid) || 
                                                        (x.Target.ParentGuid == b.ParentGuid || x.Source.ParentGuid == b.ParentGuid)).ToList();
            var listBLinkFilter = portBLinks.Where(x => (x.Target.ParentGuid == a.ParentGuid || x.Source.ParentGuid == a.ParentGuid) || 
                                                        (x.Target.ParentGuid == b.ParentGuid || x.Source.ParentGuid == b.ParentGuid)).ToList();

            bool linkAPortCheck = listALinkFilter.Any(link =>
                (link.Source.ParentGuid == a.ParentGuid && link.Source.Guid == a.Guid) && (link.Target.ParentGuid == b.ParentGuid && link.Target.Guid == b.Guid) ||
                (link.Source.ParentGuid == b.ParentGuid && link.Source.Guid == b.Guid) && (link.Target.ParentGuid == a.ParentGuid && link.Target.Guid == a.Guid)
            );
            bool linkBPortCheck = listBLinkFilter.Any(link =>
                (link.Source.ParentGuid == a.ParentGuid && link.Source.Guid == a.Guid) && (link.Target.ParentGuid == b.ParentGuid && link.Target.Guid == b.Guid) ||
                (link.Source.ParentGuid == b.ParentGuid && link.Source.Guid == b.Guid) && (link.Target.ParentGuid == a.ParentGuid && link.Target.Guid == a.Guid)
            );

            return linkAPortCheck || linkBPortCheck;
        }
        public List<(BasePort Source, BasePort Target)> GetAllLinks()
        {
            List<(BasePort Source, BasePort Target)> currentLinks = new();
            Links.ToList().ForEach(link =>
            {
                var portTarget = link.Target.Model as BasePort;
                var portSource = link.Source.Model as BasePort;

                if (portTarget != null && portSource != null)
                {
                    (BasePort Source, BasePort Target) obj = (portSource, portTarget);
                    currentLinks.Add(obj);
                }
            });
            return currentLinks ?? new();
        }

        public override bool CanAttachTo(ILinkable other)
        {

            if (!base.CanAttachTo(other)) // default constraints
                return false;
            if (other is not BasePort targetPort) // can only connect to other ports
                return false;
            if (this.Parent == targetPort.Parent) // can't connect to self
                return false;
            if (Input == targetPort.Input) // can't connect input to input or output to output
                return false;
            if (DoesLinkExist(targetPort, this))  // Check for duplicate links
                return false;

            PortType pt= this.GetPortType();
            PortType tpt = targetPort.GetPortType();

            if (pt == PortType.Multi || tpt == PortType.Multi)
                return true;
            else if(pt != tpt) 
                return false;
            return true;
        }

        #endregion
        public string? GetPortStyle()
        {
            string style;
            
            if (IsOverridden)
            {
                style = PortTypeToCssClass(_portTypeOverride);
            }
            else
            {
                style = PortTypeToCssClass(PortType);
            }

            return $"port {style} {(HasError ? "error" : string.Empty)}";
        }
        
        private string PortTypeToCssClass(PortType type)
        {
            switch (type)
            {
                case PortType.Multi:
                    return string.Empty;
                case PortType.Numeric:
                    return "numeric";
                case PortType.Logic:
                    return "logic";
                case PortType.String:
                    return "string";
                default:
                    return string.Empty;
            }
        }

        #region Port DataType
        public PortType GetPortType()
        {
            if (_portTypeOverride != PortType.None)
            {
                return _portTypeOverride;
            }
            else
            {
                return PortType;
            }
        }

        public PortType PortType { get; } 
        private PortType _portTypeOverride;
        public bool IsOverridden => _portTypeOverride != PortType;
        public PortType GetPortTypeOverride() => _portTypeOverride;
        public bool IsOverrideLeader = false;
        public void SetPortTypeOverride(PortType type)
        {
            if (_portTypeOverride != type)
            {
                _portTypeOverride = type;
            }
            OnPortTypeChanged?.Invoke(Guid, _portTypeOverride);

        }

        protected void ResetPortTypeOverride()
        {
            if (_portTypeOverride != PortType)
            {
                _portTypeOverride = PortType;
            }
            OnPortTypeChanged?.Invoke(Guid, _portTypeOverride);

        }
        
        #endregion
        
        
        
        
        
        
        public virtual void OnDispose()
        {

        }

        public void Dispose()
        {
            OnDispose();
            _portWatcher.Dispose();
            this.Changed -= PortUpdated;
        }

        
    }
}
