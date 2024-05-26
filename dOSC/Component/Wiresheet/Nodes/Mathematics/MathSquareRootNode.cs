﻿using LiteDB;
using LiveSheet;
using LiveSheet.Parts.Nodes;
using LiveSheet.Parts.Ports;

namespace dOSC.Component.Wiresheet.Nodes.Mathematics;

public class MathSquareRootNode: MathNode
{
    public MathSquareRootNode() : base()
    {
        AddPort(new LiveNumericPort(this, true, name: "Input"));
        AddPort(new LiveNumericPort(this, false, name: "Output"));
    }

    public override string NodeName => "Square Root";
    public override string Icon => "fa-solid fa-square-root-variable";
    
    public override void Process(List<EffectedNode>? effectedNodes = null)
    {
        var inA = Ports[0];
        if (inA is LiveNumericPort a
            && this.OkToProcess(effectedNodes))
        {
            BsonValue aVal = a.HasLinks() ? a.GetBsonValue()  : new(0.0);
            Value = Math.Sqrt(aVal.AsDouble);
        }
        else
        {
            Value = NodeDefault;
        }
    }
}