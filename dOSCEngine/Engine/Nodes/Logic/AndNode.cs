﻿using Blazor.Diagrams.Core.Anchors;
using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Models.Base;
using dOSCEngine.Engine.Ports;
using Newtonsoft.Json;

namespace dOSCEngine.Engine.Nodes.Logic
{
    public class AndNode : BaseNode
    {
        public AndNode(Point? position = null) : base(position ?? new Point(0, 0))
        {
            AddPort(new LogicPort(PortGuids.Port_1, this, true));
            AddPort(new LogicPort(PortGuids.Port_2, this, true));
            AddPort(new LogicPort(PortGuids.Port_3, this, false));
        }
        public AndNode(Guid guid, Point? position = null) : base(guid, position ?? new Point(0, 0))
        {
            AddPort(new LogicPort(PortGuids.Port_1, this, true));
            AddPort(new LogicPort(PortGuids.Port_2, this, true));
            AddPort(new LogicPort(PortGuids.Port_3, this, false));
        }
        [JsonProperty]
        public override string NodeClass => GetType().Name.ToString();
        public override string BlockTypeClass => "logicblock";

        public override void Refresh()
        {
            var inA = Ports[0];
            var inB = Ports[1];
            if (inA.Links.Any() && inB.Links.Any())
            {
                var l1 = inA.Links.First();
                var l2 = inB.Links.First();
                var ValA = GetInputValue(inA, l1);
                var ValB = GetInputValue(inB, l2);

                if(ValA != null && ValB != null)
                {
                    Value = (bool)ValA && (bool)ValB;
                }
            }
            else
            {
                Value = false;
            }
            base.Refresh();
        }
    }
}
