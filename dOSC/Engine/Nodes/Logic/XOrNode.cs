﻿using Blazor.Diagrams.Core.Anchors;
using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Models.Base;
using dOSC.Engine.Ports;
using System.Net.NetworkInformation;

namespace dOSC.Engine.Nodes.Logic
{
    public class XOrNode : BaseNode
    {
        public XOrNode(Point? position = null) : base(position ?? new Point(0, 0))
        {
            AddPort(new LogicPort(this, false));
            AddPort(new LogicPort(this, false));
            AddPort(new LogicPort(this, true));
        }
        public override string BlockTypeClass => "logicblock";

        public override void Refresh()
        {
            var inA = Ports[0];
            var inB = Ports[1];
            if (inA.Links.Any() && inB.Links.Any())
            {
                var l1 = inA.Links.First();
                var l2 = inB.Links.First();
                Value = GetInputValue(inA, l1) || GetInputValue(inB, l2);
            }
            else
            {
                Value = false;
            }
            base.Refresh();
        }
        private static bool GetInputValue(PortModel port, BaseLinkModel link)
        {
            var sp = (link.Source as SinglePortAnchor)!;
            var tp = (link.Source as SinglePortAnchor)!;
            var p = sp.Port == port ? tp : sp;
            return (p.Port.Parent as BaseNode)!.Value;
        }
    }
}
