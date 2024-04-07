﻿using System.Collections.Concurrent;
using Blazor.Diagrams.Core.Geometry;
using dOSC.Client.Engine.Ports;
using dOSC.Shared.Models.Wiresheet;

namespace dOSC.Client.Engine.Nodes.Mathematics;

public class SubtractNode : BaseNode
{
    public SubtractNode(Guid? guid = null, ConcurrentDictionary<EntityPropertyEnum, dynamic>? properties = null,
        Point? position = null) : base(guid, position, properties)
    {
        AddPort(new NumericPort(PortGuids.Port_1, this, true, "Value A"));
        AddPort(new NumericPort(PortGuids.Port_2, this, true, "Value B"));
        AddPort(new NumericPort(PortGuids.Port_3, this, false, "Output"));
    }

    public override string Name => "Subtract";
    public override string Category => NodeCategoryType.Math;
    public override string Icon => "icon-minus";

    public override void CalculateValue()
    {
        var i1 = Ports[0];
        var i2 = Ports[1];


        if (i1.Links.Count > 0 && i2.Links.Count > 0)
        {
            var l1 = i1.Links[0];
            var l2 = i2.Links[0];
            var v1 = GetInputValue(i1, l1);
            var v2 = GetInputValue(i2, l2);
            Value = v1 - v2;
        }
        else if (i1.Links.Count > 0)
        {
            var l1 = i1.Links[0];
            var v1 = GetInputValue(i1, l1);

            Value = v1;
        }
        else if (i2.Links.Count > 0)
        {
            var l2 = i2.Links[0];
            var v2 = GetInputValue(i2, l2);

            Value = v2;
        }
        else
        {
            Value = 0;
        }
    }
}