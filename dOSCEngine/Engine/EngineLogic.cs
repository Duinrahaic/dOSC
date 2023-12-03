using Blazor.Diagrams.Core.Anchors;
using Blazor.Diagrams.Core.Models.Base;
using Blazor.Diagrams.Core;
using dOSCEngine.Engine.Nodes;
using dOSCEngine.Engine.Ports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blazor.Diagrams;
using dOSCEngine.Services;
using Blazor.Diagrams.Core.Models;
using dOSCEngine.Engine.Links;
using ApexCharts;
using System.Xml.Linq;

namespace dOSCEngine.Engine
{
	public class EngineLogic: IDisposable
	{
        public BlazorDiagram diagram;
        public dOSCService dOSC;
        public dOSCWiresheet wiresheet;
        private bool _Built = false;
        private bool _IsPlaying => _Built;

		public EngineLogic(dOSCWiresheet wiresheet, dOSCService dOSC)
		{     
            diagram = new BlazorDiagram(dOSCWiresheetConfiguration.Options);
            diagram.RegisterBlocks();
            this.wiresheet = wiresheet;
            this.dOSC = dOSC;
        }

        public void Load(dOSCWiresheet wiresheet)
        {
            if (!_Built)
            {
                diagram.Nodes.Added += OnNodeAdded;
                diagram.Nodes.Removed += OnNodeRemoved;
                diagram.Links.Added += OnLinkAdded;
                diagram.Links.Removed += OnLinkRemoved;
                foreach (var node in wiresheet.GetAllNodes())
                {
                    diagram.Nodes.Add(node);
                }
                foreach (BaseLink l in wiresheet.GetAllLinks())
                {
                    var BlockSource = diagram.Nodes.FirstOrDefault(x => (x as BaseNode)?.Guid == l.SourcePort.ParentGuid);
                    var BlockTarget = diagram.Nodes.FirstOrDefault(x => (x as BaseNode)?.Guid == l.TargetPort.ParentGuid);

                    if (BlockSource != null && BlockTarget != null)
                    {
                        var SourcePort = BlockSource.Ports.FirstOrDefault(x => (x as BasePort)?.Guid == l.SourcePort.Guid);
                        var TargetPort = BlockTarget.Ports.FirstOrDefault(x => (x as BasePort)?.Guid == l.TargetPort.Guid);
                        if (SourcePort != null && TargetPort != null)
                        {
                            var Link = new LinkModel(SourcePort, TargetPort);
                            diagram.Links.Add(Link);
                        }
                    }
                }


                _Built = true;
            }
        }

        public void Unload()
        {
            diagram.Nodes.Added -= OnNodeAdded;
            diagram.Nodes.Removed -= OnNodeRemoved;
            diagram.Links.Added -= OnLinkAdded;
            diagram.Links.Removed -= OnLinkRemoved;
            diagram.ContainerChanged -= Diagram_ContainerChanged;
            diagram.Nodes.Clear();
            diagram.Links.Clear();
            diagram.Refresh();
        }


        private void Diagram_ContainerChanged()
        {
            try
            {
                diagram.ZoomToFit(200);
            }
            catch (Exception ex)
            {

            }

            diagram.ContainerChanged -= Diagram_ContainerChanged;
        }
        private void OnNodeAdded(NodeModel node)
        {
            var n = node as BaseNode;
            n!.ValueChanged += OnValueChanged;
        }
        private void OnNodeRemoved(NodeModel node)
        {
            var n = node as BaseNode;
            n!.ValueChanged -= OnValueChanged;
        }
        private void OnValueChanged(BaseNode op)
        {
            if (diagram != null)
            {
                foreach (var link in op.Ports.Where(x => !(x as BasePort).Input).SelectMany(x => x.Links))
                {

                    // Future Duin: Prevent links with errors from executing

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
                foreach (var node in diagram.Nodes)
                {
                    node.Refresh();
                }
            }

        }
        private void OnLinkAdded(BaseLinkModel link)
        {
            link.TargetChanged += OnLinkTargetChanged;
            var sp = (link.Source as SinglePortAnchor)!;
            var tp = (link.Target as SinglePortAnchor)!;

            if (sp != null && tp != null)
            {

            }
            if (sp != null && tp != null)
            {
                bool IsCircular = new GraphUtilities().CheckForCircularLinks(diagram);
                if (!IsCircular)
                {
                    var InputPort = (sp.Port as BasePort)!.Input ? sp : tp;
                    if (InputPort != null)
                    {
                        (InputPort.Port.Parent as BaseNode)!.CalculateValue();

                    }
                }
            }

            foreach (var node in diagram.Nodes)
            {
                node.Refresh();
            }
        }
        private void OnLinkRemoved(BaseLinkModel link)
        {
            try
            {
                diagram.Links.Remove(link);
            }
            catch (Exception ex)
            {

            }
            link.TargetChanged -= OnLinkTargetChanged;
            (link.Source.Model as PortModel)!.Parent.Refresh();
            var s = (link.Source.Model as BasePort);
            var t = (link.Target.Model as BasePort);
            if (link.Target.Model != null)
            {
                var Port = (link.Target.Model as BasePort)!;
                var Node = (Port.Parent as BaseNode)!;
                //Node.ResetValue();
            }
            foreach (var node in diagram.Nodes)
            {
                node.Refresh();
            }
        }
        private void OnLinkTargetChanged(BaseLinkModel link, Anchor? oldTarget, Anchor? newTarget)
        {
            var sp = (link.Source as SinglePortAnchor)!;
            var tp = (link.Target as SinglePortAnchor)!;

            if (sp != null && tp != null)
            {
                bool IsCircular = new GraphUtilities().CheckForCircularLinks(diagram);
                if (IsCircular)
                {
                    diagram.Links.Remove(link);
                }
            }

            if (oldTarget.Model == null && newTarget.Model != null) // First attach
            {
                (newTarget.Model as BasePort)!.Parent.Refresh();
            }
        }
        public void Dispose()
        {
            diagram.Nodes.Added -= OnNodeAdded;
            diagram.Nodes.Removed -= OnNodeRemoved;
            diagram.Links.Added -= OnLinkAdded;
            diagram.Links.Removed -= OnLinkRemoved;
        }
    }
}
