using Blazor.Diagrams;
using Blazor.Diagrams.Core.Anchors;
using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Models.Base;
using dOSC.Components.Modals;
using dOSCEngine.Components.Modals;
using dOSCEngine.Engine;
using dOSCEngine.Engine.Links;
using dOSCEngine.Engine.Nodes;
using dOSCEngine.Engine.Nodes.Connector.Activity;
using dOSCEngine.Engine.Nodes.Connector.OSC;
using dOSCEngine.Engine.Nodes.Connector.VRChat;
using dOSCEngine.Engine.Nodes.Constant;
using dOSCEngine.Engine.Nodes.Logic;
using dOSCEngine.Engine.Nodes.Math;
using dOSCEngine.Engine.Nodes.Utility;
using dOSCEngine.Engine.Ports;
using dOSCEngine.Services;
using dOSCEngine.Services.Connectors.Activity.Pulsoid;
using dOSCEngine.Services.Connectors.OSC;
using dOSCEngine.Utilities;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using static System.Windows.Forms.LinkLabel;
using Point = Blazor.Diagrams.Core.Geometry.Point;

namespace dOSC.Pages
{
    public partial class AppEditor : IDisposable
    {
        [Parameter]
        public Guid? AppId { get; set; }
        [Parameter]
        public dOSCWiresheet? Wiresheet { get; set; }
        [Inject]
        public OSCService? _OSC { get; set; }
        [Inject]
        public PulsoidService _Pulsoid { get; set; }
        [Inject]
        public IJSRuntime _JS {  get; set; }

        private BlazorDiagram diagram { get; set; }

        [Inject]
        public dOSCService? _Engine { get; set; }
        [Inject]
        public NavigationManager? NM { get; set; }

        private ModalV2 SaveModal { get; set; }
        private ModalV2 NodeSettingsModal { get; set; }

        private bool PreviouslyPlaying = false;
        private bool InitialZoomToFit = false;
        protected override void OnInitialized()
        {
            //
            //
            if (_Engine == null) return;
            if (AppId.HasValue)
            {
                Wiresheet = _Engine.GetWiresheet(AppId.Value);
                if (Wiresheet == null)
                {
                    Wiresheet = new(AppId.Value);
                }

                this.StateHasChanged();
            }
            else
            {
                Wiresheet = new(Guid.NewGuid());
            }
            if (Wiresheet == null) return;

            PreviouslyPlaying = Wiresheet.IsPlaying;

            Wiresheet.Deconstruct();

            diagram = new BlazorDiagram(dOSCWiresheetConfiguration.Options);
            diagram.RegisterBlocks();


            diagram.Nodes.Added += OnNodeAdded;
            diagram.Nodes.Removed += OnNodeRemoved;
            diagram.Links.Added += OnLinkAdded;
            diagram.Links.Removed += OnLinkRemoved;

            List<BaseNode> NodesLocal = new();
            Wiresheet.GetAllNodes().ForEach(n => {

                var NodeCopy = _Engine.ConvertNode(n.GetDTO());
                if (NodeCopy != null)
                {
                    NodeCopy.Guid = n.Guid;
                    NodesLocal.Add(diagram.Nodes.Add(NodeCopy));
                }

            });
            foreach (BaseLink l in Wiresheet.GetAllLinks().DistinctBy(x => x.Guid))
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

            diagram.Refresh();


            this.diagram.ContainerChanged += Diagram_ContainerChanged;
            
        }

        private void Diagram_ContainerChanged()
        {
            try
            {
                this.diagram.ZoomToFit(200);
            }
            catch(Exception ex) 
            {

            }

            this.diagram.ContainerChanged -= Diagram_ContainerChanged;
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
 

        public void OnValueChanged(BaseNode op)
        {
            if (diagram != null)
            {
                foreach (var link in op.Ports.Where(x=> !(x as BasePort).Input).SelectMany(x=>x.Links))
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
                foreach(var node in diagram.Nodes)
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

            if(sp != null && tp != null)
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
            catch(Exception ex)
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
                     _JS.InvokeVoidAsync("GenerateToasterMessage", "Infinite/Circular Link Detected! Removing last link!").ConfigureAwait(false);

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

        
        private void Save()
        {
            if (Wiresheet == null) return;
            SaveModal.Open();

        }

        private void SaveApp(EditContext context)
        {
            SaveModal.Close();
            if (Wiresheet == null) return;
            if (_Engine == null) return;

            Wiresheet._Links.Clear();
            Wiresheet._Nodes.Clear();
            diagram.Nodes.ToList().ForEach(node =>
            {
                var n = node as BaseNode;
                Wiresheet._Nodes.Add(n);
            });
            diagram.Links.ToList().ForEach(link =>
            {
                var s = link.Source.Model as BasePort;
                var t = link.Target.Model as BasePort;

                if(diagram.Nodes.Any(x=> (x as BaseNode)?.Guid == s!.ParentGuid) == false 
                    || Wiresheet._Nodes.Any(x=> x.Guid == t!.ParentGuid) == false)
                {

                }
                else
                {
                    Wiresheet._Links.Add(new(s, t));
                }


            });
            _Engine.SaveWiresheet(Wiresheet);
        }

        private void Revert()
        {
            if (_Engine == null) return;
            if (Wiresheet == null) return;
            NM!.NavigateTo($"/apps/");
            NM!.NavigateTo($"/apps/editor/{Wiresheet.AppGuid}");
        }
        private async Task DownloadApp()
        {
            if (Wiresheet != null)
            {

                await FileSystem.DownloadWiresheet(_JS, Wiresheet);
            }
        }

        private void Exit()
        {
            if (Wiresheet == null) return;
            if (_Engine == null) return;
            diagram.SuspendRefresh = true;
            if (PreviouslyPlaying)
            {
                Wiresheet.Build();
            }
            else
            {
                Wiresheet.Deconstruct();
            }
            NM!.NavigateTo($"/apps/{Wiresheet.AppGuid}");
        }
    }
}
