using Blazor.Diagrams;
using Blazor.Diagrams.Core.Anchors;
using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Models.Base;
using dOSC.Engine.Links;
using dOSC.Engine.Nodes;
using dOSC.Engine.Ports;
using Newtonsoft.Json;

namespace dOSC.Services
{
    public partial class dOSCWiresheet : IDisposable
    {
        [JsonIgnore]        
        public BlazorDiagram BlazorDiagram { get; set; }

        public List<BaseNode> _Nodes = new List<BaseNode>();
        public List<BaseLink> _Links = new List<BaseLink>();
        private bool _Built = false;
        public bool IsPlaying {
            get 
            { 
                return !BlazorDiagram.SuspendRefresh;
            }
            set 
            {
                BlazorDiagram.SuspendRefresh = value;
            }
        }
        public bool HasError { get; set; } = false;


        

        public void Build()
        {

            if (!_Built)
            {
                
                BlazorDiagram.Nodes.Added += OnNodeAdded;
                BlazorDiagram.Nodes.Removed += OnNodeRemoved;
                BlazorDiagram.Links.Added += OnLinkAdded;
                BlazorDiagram.Links.Removed += OnLinkRemoved;
                foreach (var node in _Nodes)
                {
                    BlazorDiagram.Nodes.Add(node);
                }

                foreach (var r in _Links)
                {
                    ///The connection point will be the intersection of
                    // a line going from the target to the center of the source
                    var sourceAnchor = new SinglePortAnchor(r.SourcePort);
                    // The connection point will be the port's position
                    var targetAnchor = new SinglePortAnchor(r.TargetPort);
                    var link = BlazorDiagram.Links.Add(new LinkModel(sourceAnchor, targetAnchor));
                }

                _Built = true;
            }
        }
        public void Desconstruct()
        {
            if (_Built)
            {
                BlazorDiagram.Nodes.Added -= OnNodeAdded;
                BlazorDiagram.Nodes.Removed -= OnNodeRemoved;
                BlazorDiagram.Links.Added -= OnLinkAdded;
                BlazorDiagram.Links.Removed -= OnLinkRemoved;
                BlazorDiagram.SuspendRefresh = true;
                BlazorDiagram = new(dOSCWiresheetConfiguration.Options);
                BlazorDiagram.RegisterBlocks();
                _Built = false;
            }

        }
        public void Start()
        {
            BlazorDiagram.SuspendRefresh = false;
        }
        public void Stop()
        {
            BlazorDiagram.SuspendRefresh = true;
        }


        public void AddNode(BaseNode node)
        {
            _Nodes.Add(node);
            BlazorDiagram.Nodes.Add(node);
        }

        public void AddRelationship(BasePort source, BasePort target)
        {
            BaseLink link = new(source, target);
            _Links.Add(link);
            BlazorDiagram.Links.Add(link);
        }

        public List<BaseNode> GetAllNodes()
        {
            return _Nodes;
        }
        public List<BaseLink> GetAllLinks()
        {
            return _Links;
        }

        private void OnNodeAdded(NodeModel node)
        {
            (node as BaseNode)!.ValueChanged += OnValueChanged;
        }
        private void OnNodeRemoved(NodeModel node)
        {
            (node as BaseNode)!.ValueChanged -= OnValueChanged;
        }

        public void OnValueChanged(BaseNode op)
        {
            if(BlazorDiagram != null)
            {
                foreach (var link in BlazorDiagram.Links)
                {
                    var sp = (link.Source as SinglePortAnchor)!;
                    var tp = (link.Target as SinglePortAnchor)!;
                    if (sp != null && tp != null)
                    {
                        var otherNode = sp.Port.Parent == op ? tp.Port.Parent : sp.Port.Parent;
                        otherNode.Refresh();

                    }
                }
            }
            
        }

        private void OnLinkAdded(BaseLinkModel link)
        {
            link.TargetChanged += OnLinkTargetChanged;
        }
        private void OnLinkRemoved(BaseLinkModel link)
        {
            (link.Source.Model as PortModel)!.Parent.Refresh();

            if (link.Target.Model != null)
            {
                var Port = (link.Target.Model as PortModel)!;
                var Node = (Port.Parent as BaseNode)!;
                Node.ResetValue();
                Node.Refresh(); 
            }
            link.TargetChanged -= OnLinkTargetChanged;
        }


        private void OnLinkTargetChanged(BaseLinkModel link, Anchor? oldTarget, Anchor? newTarget)
        {
            if (oldTarget.Model == null && newTarget.Model != null) // First attach
            {
                (newTarget.Model as PortModel)!.Parent.Refresh();
            }
        }


        public void Dispose()
        {
            BlazorDiagram.Nodes.Added -= OnNodeAdded;
            BlazorDiagram.Nodes.Removed -= OnNodeRemoved;
            BlazorDiagram.Links.Added -= OnLinkAdded;
            BlazorDiagram.Links.Removed -= OnLinkRemoved;
        }
    }
}
