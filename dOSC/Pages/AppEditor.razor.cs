﻿using Blazor.Diagrams;
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

        private BlazorDiagram diagram { get; set; }

        [Inject]
        public dOSCEngine? _Engine { get; set; }
        [Inject]
        public NavigationManager? NM { get; set; }

        private List<BaseNode> Nodes = new();
        private List<BaseLink> Links = new();


        protected override void OnParametersSet()
        {
            
            //Wiresheet.Build();

        }


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

            Wiresheet.GetAllNodes().ForEach(n => { diagram.Nodes.Add(n); });
            Wiresheet.GetAllLinks().ForEach(n => { diagram.Links.Add(n); });
        }

        private void OnNodeAdded(NodeModel node)
        {
            var n = node as BaseNode;
            n!.ValueChanged += OnValueChanged;
            Nodes.Add(n);
        }
        private void OnNodeRemoved(NodeModel node)
        {
            var n = node as BaseNode;
            n!.ValueChanged -= OnValueChanged;
            Nodes.Remove(n);

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
            var s = (link.Source.Model as BasePort);
            var t = (link.Target.Model as BasePort);

            link.TargetChanged += OnLinkTargetChanged;
            
            if(s != null && t != null)
                Links.Add(new(s, t));
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
            if (s != null && t != null)
                Links.RemoveAll(x=> x.SourcePort.ParentGuid == s.ParentGuid && x.TargetPort.ParentGuid == t.ParentGuid);
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
            if (_Engine == null) return;

            Wiresheet._Links.Clear();
            Wiresheet._Nodes.Clear();
            Wiresheet._Nodes.AddRange(Nodes);
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
        private void OSCButton() => diagram.Nodes.Add(new OSCVRCButtonNode(service:_OSC, position: CenterOfScreen()));

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
        private void Add() => diagram.Nodes.Add(new AddNode(position: CenterOfScreen()));
        private void Subtract() => diagram.Nodes.Add(new SubtractNode(position: CenterOfScreen()));
        private void Multiply() => diagram.Nodes.Add(new MultiplicationNode(position: CenterOfScreen()));
        private void Divide() => diagram.Nodes.Add(new DivisionNode(position: CenterOfScreen()));
        #endregion

        #region Utility
        private void Sine() => diagram.Nodes.Add(new SineNode(position: CenterOfScreen()));

        #endregion




    }
}
