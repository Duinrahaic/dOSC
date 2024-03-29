﻿using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using dOSCEngine.Engine.Ports;
using Newtonsoft.Json;
using System.Collections.Concurrent;

namespace dOSCEngine.Engine.Nodes.Mathematics
{
    public class MultiplicationNode : BaseNode
    {

        public MultiplicationNode(Guid? guid = null, ConcurrentDictionary<EntityPropertyEnum, dynamic>? properties = null, Point? position = null) : base(guid, position, properties)
        {
            AddPort(new NumericPort(PortGuids.Port_1, this, true, name: "Numerator"));
            AddPort(new NumericPort(PortGuids.Port_2, this, true, name: "Denominator"));
            AddPort(new NumericPort(PortGuids.Port_3, this, false, name: "Output"));
        }
        
        public override string Name => "Multiply";
        public override string Category => NodeCategoryType.Math;
        public override string Icon => "icon-x";
        public override void CalculateValue()
        {
            var i1 = Ports[0];
            var i2 = Ports[1];
            if (i1.Links.Any() && i2.Links.Any())
            {
                var v1 = GetInputValue(i1, i1.Links.First());
                var v2 = GetInputValue(i2, i2.Links.First());
                Value = v1 * v2;
            }
            else if (i1.Links.Any())
            {
                var v1 = GetInputValue(i1, i1.Links.First());
                Value = v1;
            }
            else if (i2.Links.Any())
            {
                var v2 = GetInputValue(i2, i2.Links.First());
                Value = v2;
            }
            else
            {
                Value = 0;
            }
        }

    }
}
