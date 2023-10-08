using Blazor.Diagrams;
using Blazor.Diagrams.Core.Anchors;
using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core;
using Blazor.Diagrams.Core.PathGenerators;
using Blazor.Diagrams.Core.Routers;
using Blazor.Diagrams.Options;
using dOSC.Engine.Nodes;
using Blazor.Diagrams.Core.Models.Base;
using dOSC.Components.Wiresheet.Blocks.Connectors.OSC.Button;
using dOSC.Components.Wiresheet.Blocks.Logic;
using dOSC.Components.Wiresheet.Blocks.Math;
using dOSC.Engine.Nodes.Connector.OSC;
using dOSC.Engine.Nodes.Logic;
using dOSC.Engine.Nodes.Math;
using dOSC.Components;

namespace dOSC.Services
{
    public class dOSCWiresheet : IDisposable
    {
        public Guid AppGuid { get; set; } = Guid.NewGuid();
        public BlazorDiagram BlazorDiagram { get; set; } = new(dOSCWiresheetConfiguration.Options);
        private HashSet<(string Id, NodeModel Node)> _Nodes = new HashSet<(string, NodeModel)>();
        private HashSet<(string Id, PortModel Source,PortModel Target)> _Links = new HashSet<(string, PortModel, PortModel)>();
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
        public dOSCWiresheet()
        {
            BlazorDiagram.RegisterBlocks();
            BlazorDiagram.SuspendRefresh = true;
        }

        public dOSCWiresheet(Guid AppGuid) : base()
        {
            this.AppGuid = AppGuid;
        }


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
                    BlazorDiagram.Nodes.Add(node.Node);
                }

                foreach (var r in _Links)
                {
                    ///The connection point will be the intersection of
                    // a line going from the target to the center of the source
                    var sourceAnchor = new SinglePortAnchor(r.Source);
                    // The connection point will be the port's position
                    var targetAnchor = new SinglePortAnchor(r.Target);
                    var link = BlazorDiagram.Links.Add(new LinkModel(sourceAnchor, targetAnchor));
                }

                _Built = true;
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

        public void Desconstruct()
        {
            if(_Built)
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

        public void AddNode(NodeModel node)
        {
            _Nodes.Add((node.Id, node));
        }

        public void AddRelationship(PortModel source, PortModel target)
        {
            _Links.Add(($"{source.Id}_{target.Id}", source, target));
        }

        public HashSet<(string Id, NodeModel Node)> GetAllNodes()
        {
            return _Nodes;
        }
        public HashSet<(string Id, PortModel Source, PortModel Target)> GetAllLinks()
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
