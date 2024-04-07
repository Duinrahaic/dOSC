﻿using System.Collections.Concurrent;
using Blazor.Diagrams.Core.Geometry;
using dOSC.Client.Engine.Ports;
using dOSC.Shared.Models.Wiresheet;

namespace dOSC.Client.Engine.Nodes.Mathematics;

public class AbsoluteNode : BaseNode
{
    public AbsoluteNode(Guid? guid = null, ConcurrentDictionary<EntityPropertyEnum, dynamic>? properties = null,
        Point? position = null) : base(guid, position, properties)
    {
        AddPort(new NumericPort(PortGuids.Port_1, this, true, "Value"));
        AddPort(new NumericPort(PortGuids.Port_2, this, false, "Output"));
    }

    public override string Name => "Absolute";
    public override string Category => NodeCategoryType.Math;
    public override string TextIcon => "|x|";

    public override void CalculateValue()
    {
        var input = Ports[0];
        if (!input.Links.Any())
        {
            SetValue(null!, false);
        }
        else
        {
            double? LV = GetInputValue(input, input.Links.First());


            if (LV.HasValue)
                Value = Math.Abs(LV.Value);
            else
                SetValue(null!, false);
        }
    }
}