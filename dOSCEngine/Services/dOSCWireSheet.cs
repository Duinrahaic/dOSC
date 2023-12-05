using Blazor.Diagrams;
using Blazor.Diagrams.Core;
using Blazor.Diagrams.Core.Anchors;
using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Models.Base;
using dOSCEngine.Engine.Links;
using dOSCEngine.Engine.Nodes;
using dOSCEngine.Engine.Nodes.Constant;
using dOSCEngine.Engine.Nodes.Logic;
using dOSCEngine.Engine.Ports;
using Newtonsoft.Json;

namespace dOSCEngine.Services
{
    public partial class dOSCWiresheet : IDisposable
    {
        [JsonIgnore]        
        public BlazorDiagram Diagram { get; set; }
        public List<BaseNode> _Nodes = new List<BaseNode>();
        public List<BaseLink> _Links = new List<BaseLink>();
        private bool _Built = false;
        public bool IsPlaying => _Built;
        public bool HasError { get; set; } = false;
        public void Build()
        {

            if (!_Built)
            {
                
                Diagram.Nodes.Added += OnNodeAdded;
                Diagram.Nodes.Removed += OnNodeRemoved;
                Diagram.Links.Added += OnLinkAdded;
                Diagram.Links.Removed += OnLinkRemoved;
                foreach (var node in _Nodes)
                {
                    Diagram.Nodes.Add(node);
                }

                foreach (BaseLink l in _Links)
                {
                    var BlockSource = Diagram.Nodes.FirstOrDefault(x => (x as BaseNode)?.Guid == l.SourcePort.ParentGuid);
                    var BlockTarget = Diagram.Nodes.FirstOrDefault(x => (x as BaseNode)?.Guid == l.TargetPort.ParentGuid);

                    if (BlockSource != null && BlockTarget != null)
                    {
                        var SourcePort = BlockSource.Ports.FirstOrDefault(x => (x as BasePort)?.Guid == l.SourcePort.Guid);
                        var TargetPort = BlockTarget.Ports.FirstOrDefault(x => (x as BasePort)?.Guid == l.TargetPort.Guid);
                        if (SourcePort != null && TargetPort != null)
                        {
                            var Link = new LinkModel(SourcePort, TargetPort);
                            Diagram.Links.Add(Link);
                        }
                    }
                }


                _Built = true;
            }
        }
        public void Deconstruct()
        {
            if (_Built)
            {
                foreach(var node in _Nodes)
                {
                    Diagram.Nodes.Remove(node);
                }
                Diagram.Nodes.Added -= OnNodeAdded;
                Diagram.Nodes.Removed -= OnNodeRemoved;
                Diagram.Links.Added -= OnLinkAdded;
                Diagram.Links.Removed -= OnLinkRemoved;
                //Diagram.SuspendRefresh = true;
                Diagram = new(dOSCWiresheetConfiguration.Options);
                Diagram.RegisterBlocks();
                _Built = false;
            }

        }
        public void AddNode(BaseNode node)
        {
            _Nodes.Add(node);
            Diagram.Nodes.Add(node);
        }

        public void AddRelationship(BasePort source, BasePort target)
        {
            BaseLink link = new(source, target);
            _Links.Add(link);
            Diagram.Links.Add(link as LinkModel);
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
            if (Diagram != null)
            {
                foreach (var link in op.Ports.Where(x => !(x as BasePort).Input).SelectMany(x => x.Links))
                {

                    var sp = (link.Source as SinglePortAnchor)!;
                    var tp = (link.Target as SinglePortAnchor)!;
                    if (sp != null && tp != null)
                    {
                        var InputPort = (sp.Port as BasePort)!.Input ? sp : tp;
                        if (InputPort != null)
                        {
                            (InputPort.Port.Parent as BaseNode)!.CalculateValue();

                        }

                        //var otherNode = sp.Port.Parent == op ? tp.Port.Parent : sp.Port.Parent;
                        //otherNode.Refresh();

                    }
                }
                foreach (var node in Diagram.Nodes)
                {
                    node.Refresh();
                }
            }

        }

        private void OnLinkAdded(BaseLinkModel link)
        {
            link.TargetChanged += OnLinkTargetChanged;
        }
        private void OnLinkRemoved(BaseLinkModel link)
        {
			try
			{
				Diagram.Links.Remove(link);
			}
			catch (Exception ex)
			{

			}
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
            Diagram.Nodes.Added -= OnNodeAdded;
            Diagram.Nodes.Removed -= OnNodeRemoved;
            Diagram.Links.Added -= OnLinkAdded;
            Diagram.Links.Removed -= OnLinkRemoved;
        }
    }
}
