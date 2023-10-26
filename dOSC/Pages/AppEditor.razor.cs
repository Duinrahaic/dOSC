using Blazor.Diagrams;
using Blazor.Diagrams.Core;
using Blazor.Diagrams.Core.Anchors;
using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models.Base;
using Blazor.Diagrams.Core.Models;
using dOSC.Engine.Nodes;
using dOSC.Engine.Nodes.Connector.OSC;
using dOSC.Engine.Nodes.Constant;
using dOSC.Engine.Nodes.Logic;
using dOSC.Engine.Nodes.Math;
using dOSC.Engine.Nodes.Utility;
using dOSC.Services;
using dOSC.Services.Connectors.OSC;
using Microsoft.AspNetCore.Components;
using dOSC.Engine.Ports;
using dOSC.Engine.Links;
using dOSC.Components.Modals;
using dOSC.Services.Connectors.Activity.Pulsoid;
using dOSC.Engine.Nodes.Connector.Activity;
using dOSC.Engine.Nodes.Connector.OSC.VRChat;
using System.Xml.Linq;

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

        private BlazorDiagram diagram { get; set; }

        [Inject]
        public dOSCEngine? _Engine { get; set; }
        [Inject]
        public NavigationManager? NM { get; set; }

        private ModalBase SaveModal { get; set; }


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
            Wiresheet.Desconstruct();

            diagram = new BlazorDiagram(dOSCWiresheetConfiguration.Options);
            diagram.RegisterBlocks();


            diagram.Nodes.Added += OnNodeAdded;
            diagram.Nodes.Removed += OnNodeRemoved;
            diagram.Links.Added += OnLinkAdded;
            diagram.Links.Removed += OnLinkRemoved;


            List<BaseNode> NodesLocal = new();
            Wiresheet.GetAllNodes().ForEach(n => { NodesLocal.Add(diagram.Nodes.Add(n)); });
            foreach(BaseLink l in Wiresheet.GetAllLinks())
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

        }

        protected override void OnAfterRender(bool firstRender)
        {
            if(firstRender)
            {
                
            }
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
                foreach (var link in diagram.Links)
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
            var s = (link.Source.Model as BasePort);
            var t = (link.Target.Model as BasePort);
            if (link.Target.Model != null)
            {
                var Port = (link.Target.Model as BasePort)!;
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

        #region File
        
        private void Save()
        {
            if (Wiresheet == null) return;
            SaveModal.Open();

        }

        private void HandleOnChange(ChangeEventArgs args)
        {
            
            Wiresheet!.AppDescription = args.Value?.ToString() ?? "";
        }

        private void SaveApp()
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

        private void Exit()
        {
            if (Wiresheet == null) return;
            if (_Engine == null) return;
            diagram.SuspendRefresh = true;
            Wiresheet.Build();
            NM!.NavigateTo($"/apps/{Wiresheet.AppGuid}");
        }
        #endregion

        private Point CenterOfScreen()
        {
            if (diagram == null) return Point.Zero;
            var x = (diagram.Container.Width / 2 - diagram.Pan.X) / diagram.Zoom;
            var y = (diagram.Container.Height / 2 - diagram.Pan.Y) / diagram.Zoom;
            return new Point(x, y);
        }

        #region Connectors
        // Activity
        private void Pulsoid() => diagram.Nodes.Add(new PulsoidNode(service: _Pulsoid, position: CenterOfScreen()));

        // OSC
        private void OSCBoolean() => diagram.Nodes.Add(new OSCBooleanNode(service: _OSC, position: CenterOfScreen()));
        private void OSCReadBoolean() => diagram.Nodes.Add(new OSCBooleanReadNode(service: _OSC, position: CenterOfScreen()));
        private void OSCFloat() => diagram.Nodes.Add(new OSCFloatNode(service: _OSC, position: CenterOfScreen()));
        private void OSCReadFloat() => diagram.Nodes.Add(new OSCFloatReadNode(service: _OSC, position: CenterOfScreen()));
        private void OSCInt() => diagram.Nodes.Add(new OSCIntNode(service: _OSC, position: CenterOfScreen()));
        private void OSCReadInt() => diagram.Nodes.Add(new OSCIntReadNode(service: _OSC, position: CenterOfScreen()));

        // OSC - VRChat
        private void OSCVRCAvatarRead() => diagram.Nodes.Add(new OSCVRCAvatarReadNode(service:_OSC, position: CenterOfScreen()));
        private void OSCVRCAvatarWrite() => diagram.Nodes.Add(new OSCVRCAvatarWriteNode(service:_OSC, position: CenterOfScreen()));
        private void OSCVRCAxis() => diagram.Nodes.Add(new OSCVRCAxisNode(service:_OSC, position: CenterOfScreen()));
        private void OSCVRCChat() => diagram.Nodes.Add(new OSCVRCChatboxNode(service:_OSC, position: CenterOfScreen()));
        private void OSCVRCButton() => diagram.Nodes.Add(new OSCVRCButtonNode(service:_OSC, position: CenterOfScreen()));
        #endregion

        #region Constants
        private void Logic() => diagram.Nodes.Add(new LogicNode(position: CenterOfScreen()));
        private void Numeric() => diagram.Nodes.Add(new NumericNode(position: CenterOfScreen()));
        #endregion

        #region Logic
        private void AndBlock() => diagram.Nodes.Add(new AndNode(position: CenterOfScreen()));
        private void EqualBlock() => diagram.Nodes.Add(new EqualNode(position: CenterOfScreen()));
        private void GreaterThan() => diagram.Nodes.Add(new GreaterThanNode(position: CenterOfScreen()));
        private void GreaterThanEqual() => diagram.Nodes.Add(new GreaterThanEqualNode(position: CenterOfScreen()));
        private void LessThan() => diagram.Nodes.Add(new LessThanNode(position: CenterOfScreen()));
        private void LessThanEqual() => diagram.Nodes.Add(new LessThanEqualNode(position: CenterOfScreen()));
        private void NotEqual() => diagram.Nodes.Add(new NotEqualNode(position: CenterOfScreen()));
        private void Not() => diagram.Nodes.Add(new NotNode(position: CenterOfScreen()));
        private void Or() => diagram.Nodes.Add(new OrNode(position: CenterOfScreen()));
        private void XOr() => diagram.Nodes.Add(new XOrNode(position: CenterOfScreen()));
        #endregion

        #region Math
        private void Absolute() => diagram.Nodes.Add(new AbsoluteNode(position: CenterOfScreen()));
        private void Add() => diagram.Nodes.Add(new AddNode(position: CenterOfScreen()));
        private void Average() => diagram.Nodes.Add(new AverageNode(position: CenterOfScreen()));
        private void Clamp() => diagram.Nodes.Add(new ClampNode(position: CenterOfScreen()));
        private void Divide() => diagram.Nodes.Add(new DivisionNode(position: CenterOfScreen()));
        private void Max() => diagram.Nodes.Add(new MaxNode(position: CenterOfScreen()));
        private void Min() => diagram.Nodes.Add(new MinNode(position: CenterOfScreen()));
        private void Multiply() => diagram.Nodes.Add(new MultiplicationNode(position: CenterOfScreen()));
        private void Negative() => diagram.Nodes.Add(new NegativeNode(position: CenterOfScreen()));
        private void Power() => diagram.Nodes.Add(new PowerNode(position: CenterOfScreen()));
        private void Sine() => diagram.Nodes.Add(new SineNode(position: CenterOfScreen()));
        private void Subtract() => diagram.Nodes.Add(new SubtractNode(position: CenterOfScreen()));
        private void SquareRoot() => diagram.Nodes.Add(new SquareRootNode(position: CenterOfScreen()));
        private void Summation() => diagram.Nodes.Add(new SummationNode(position: CenterOfScreen()));

        #endregion

        #region Utility

        private void LogicSwitch() => diagram.Nodes.Add(new LogicSwitchNode(position: CenterOfScreen()));
        private void NumericSwitch() => diagram.Nodes.Add(new NumericSwitchNode(position: CenterOfScreen()));

        #endregion




    }
}
