using System.Collections.Concurrent;
using Blazor.Diagrams.Core.Anchors;
using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Models.Base;
using dOSC.Client.Engine.Ports;
using dOSC.Shared.Models.Wiresheet;
using dOSC.Shared.Utilities;
using dOSCEngine.Engine;
using dOSCEngine.Engine.Nodes;

namespace dOSC.Client.Engine.Nodes
{
    public abstract class BaseNode : NodeModel
    {
        private dynamic _value = null!;
        public event Action<BaseNode>? ValueChanged;

        public delegate void UpdateVisuals();
        public event UpdateVisuals? OnVisualUpdateRequest;
        
        public delegate void UpdateHeader();
        public event UpdateHeader? OnHeaderUpdateRequest;

        public delegate void UpdateProgressBar();
        public event UpdateProgressBar? OnProgressBarUpdateRequest;
        
        public delegate void UpdateQueueBar();
        public event UpdateQueueBar? OnQueueBarUpdateRequest;
        

        protected BaseNode(Guid? guid = null, Point? position = null, ConcurrentDictionary<EntityPropertyEnum, dynamic>? properties = null) : base(position ?? new Point(0, 0))
        {
            Guid = guid ?? Guid.NewGuid();
            Size = new Size(1, 1);

            Properties = new(properties ?? new ConcurrentDictionary<EntityPropertyEnum, dynamic>());
            Properties.OnPropertyChangeUpdate += PropertyNotifyEvent;
            var autoEvent = new AutoResetEvent(true);
            _dragWatcher = new(UpdateDragState,autoEvent, 100,100);
            Moving += DraggingEvent;
        }

       
        
        

        public virtual void PropertyNotifyEvent(EntityPropertyEnum property, dynamic? value)
        {
            
        }

        public EntityProperties Properties;
        
        
        public void SubscribeToAllPortTypeChanges()
        {
            foreach (var mp in Ports.Where(x => x is MultiPort).Select(x => x as MultiPort))
            {
                if (mp != null)
                    mp.OnPortTypeChanged += PortTypeChanged;
            }
        }

        public void UnsubscribeToAllPortTypeChanged()
        {
            foreach (var mp in Ports.Where(x => x is MultiPort).Select(x => x as MultiPort))
            {
                if (mp != null)
                    mp.OnPortTypeChanged -= PortTypeChanged;
            }
        }
        

        public List<BasePort> GetAllPorts()
        {
            List<BasePort> ports = new();

            foreach (var Port in Ports)
            {
                if(Port is BasePort)
                {
                    BasePort basePort = (BasePort)Port;
                    ports.Add(basePort);
                }
            }
            return ports;
        }

        public List<BasePort> GetAllInputs() => GetAllPorts().Where(x => x.Input == true).ToList();
        public List<BasePort> GetAllOutputs() => GetAllPorts().Where(x => x.Input == false).ToList();
        public Guid Guid { get; set; } = Guid.NewGuid();
        public virtual string NodeClass => GetType().Name;
        public virtual string Option => string.Empty;
        public string ErrorMessage { get; set; } = string.Empty;
        private bool _error = false;
        public bool Error => _error;
        public virtual string Name => string.Empty;
        
        private string _displayName = string.Empty;
        
        public string DisplayName
        {
            get => string.IsNullOrEmpty(_displayName)? Name : _displayName;
            set
            {
                if (_displayName != value)
                {
                    _displayName = value;
                    Properties.SetProperty(EntityPropertyEnum.DisplayName, value);
                    OnHeaderUpdateRequest?.Invoke();
                }
            }
        }
        
        public void ResetDisplayName()
        {
            DisplayName = string.Empty;
        }

        public void RequestHeaderUpdate()
        {
            OnVisualUpdateRequest?.Invoke();
        }
        
        
        public virtual string Category => string.Empty;
        public virtual string Icon => string.Empty;
        public virtual string TextIcon => string.Empty;
        
        public void SetErrorState(bool errorState, string errorMessage = "")
        {
            ErrorMessage = errorMessage;
            _error = errorState;
        }
        public dynamic Value
        {
            get => _value;
            set
            {
                _value = value;
                ValueChanged?.Invoke(this);
            }
        }
        public void SetValue(dynamic value, bool Refresh = true)
        {
            if (Refresh)
            {
                Value = value;
            }
            else
            {
                _value = value;
            }
        }
        public virtual void CalculateValue()
        {

        }
        
      
        protected virtual dynamic GetInputValue(PortModel port, BaseLinkModel link)
        {
            var sp = (link.Source as SinglePortAnchor)!;
            var tp = (link.Target as SinglePortAnchor)!;
            var p = sp.Port == port ? tp : sp;
            try
            {
                return (p.Port.Parent as BaseNode)!.Value;
            }
            catch
            {
                return null;
            }
        }
        public virtual void ResetValue()
        {
            Value = null!;
        }
        public double InputValue(PortModel port, BaseLinkModel link)
        {
            return GetInputValue(port, link);
        }
        
        
        
        
        
        private readonly CommitManager<PortType> _multiPortMonitor = new CommitManager<PortType>();
        private string _commitLeader = string.Empty;
        public void PortTypeChanged(Guid portGuid, PortType portType)
        {
            PortType nextPortType = PortType.Multi;
            // Apply or Remove Commits
            var port = Ports.Where(p => p is MultiPort).Select(p => p as MultiPort).FirstOrDefault(p => p!.Guid == portGuid);
            if (port != null)
                if (port.HasValidLinks() && portType != PortType.Multi)
                    _multiPortMonitor.Commit(portType, portGuid.ToString());
                else
                    _multiPortMonitor.RemoveCommit(portGuid.ToString());

            // Check for master and set Point Data DataType
            if (!_multiPortMonitor.HasCommits())
            {
                nextPortType = PortType.Multi;
                _commitLeader = String.Empty;
            }
            else
            {
                var peak = _multiPortMonitor.PeekNext();
                nextPortType = peak.data;
                _commitLeader = peak.source;

            }

            var allPorts = Ports.Where(p => p is MultiPort).ToList();
            foreach (var mp in allPorts)
            {
                var multiPort = mp as MultiPort;
                if (multiPort != null)
                {
                    if(string.IsNullOrEmpty(_commitLeader))
                        multiPort.IsOverrideLeader = false;
                    else if(multiPort.Guid.ToString().Equals(_commitLeader, StringComparison.OrdinalIgnoreCase))
                        multiPort.IsOverrideLeader = true;
                    else
                        multiPort.IsOverrideLeader = false;
                    
                    var currentPortType = multiPort.GetPortType();
                    if (currentPortType != nextPortType)
                    {
                        multiPort.SetPortTypeOverride(nextPortType);
                    }
                }
            }
        }
        
        public PortType? GetCurrentMultiPortType()
        {
            if(!Ports.Any(x=>x is MultiPort)) 
                return null;
            else if (!_multiPortMonitor.HasCommits())
                return PortType.Multi;
            else
                return _multiPortMonitor.PeekNext().data;
        }

        #region  Visuals


        public bool ShowInputField { get; set; } = false;
        public bool ShowProgressBar { get; set; } = false;
        private double _progress = 0;
        public double Progress
        {
            get => _progress;
            set
            {
                _progress = value;
                OnProgressBarUpdateRequest?.Invoke();
            }
        }


        public bool ShowQueueCount => QueueSize > 1;
        private int _itemsInQueue = 0;
        public int ItemsInQueue
        {
            get => _itemsInQueue;
            set
            {
                if (_itemsInQueue == value)
                    return;
                _itemsInQueue = value;
                OnQueueBarUpdateRequest?.Invoke();
            }
        }
        private int _queueSize = 0;
        public int QueueSize
        {
            get => _queueSize;
            set
            {
                if (_queueSize == value)
                    return;
                _queueSize = value;
                OnQueueBarUpdateRequest?.Invoke();
            }
        }
        
        private string _visualIndicator = string.Empty;

        public string VisualIndicator
        {
            get => _visualIndicator;
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    _visualIndicator = String.Empty;
                }

                if (!_visualIndicator.Equals(value))
                {
                    _visualIndicator = value;
                    OnVisualUpdateRequest?.Invoke();
                }
            }
        }

        #endregion
        
        
         
  
        public bool Dragging { get; set; } = false;
        private Timer _dragWatcher;
        private void DraggingEvent(NodeModel obj)
        {
            Dragging = true;
            _lastDrag = DateTime.Now;
        }
        private DateTime _lastDrag = DateTime.MinValue;
        private static TimeSpan _lastDragTimeDebounce = TimeSpan.FromMilliseconds(100);
        private void UpdateDragState(object _)
        {
            if (DateTime.Now - _lastDrag > _lastDragTimeDebounce && Dragging)
            {
                Dragging = false;
            }
        }
        
        
        
        public BaseNodeDTO GetDTO()
        {
            return new BaseNodeDTO()
            {
                Guid = this.Guid,
                NodeClass = this.NodeClass,
                Value = this.Value,
                Position = this.Position,
                Option = this.Option,
                Properties = this.Properties.GetAllProperties()
            };
        }

        public virtual void OnDispose()
        {

        }


        public void Dispose()
        {
            OnDispose();
            Properties.OnPropertyChangeUpdate -= PropertyNotifyEvent;
            Moving -= DraggingEvent;
            _dragWatcher.Dispose();
        }
    }
}
