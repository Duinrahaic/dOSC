using LiteDB;
using LiveSheet;
using LiveSheet.Parts.Nodes;
using LiveSheet.Parts.Ports;

namespace dOSC.Component.Wiresheet.Nodes.Mathematics;

public class MathPowerNode: MathNode
{
    public MathPowerNode() : base()
    {
        AddPort(new LiveNumericPort(this, true, name: "Input"));
        AddPort(new LiveNumericPort(this, true, name: "Power"));
        AddPort(new LiveNumericPort(this, false, name: "Output"));
    }

    public override string NodeName => "Power";
    public override string Icon => "fa-solid fa-superscript";
    
    public override void Process(List<EffectedNode>? effectedNodes = null)
    {
        var inA = Ports[0];
        var inB = Ports[1];
        if (inA is LiveNumericPort a
            && inB is LiveNumericPort b
            && this.OkToProcess(effectedNodes))
        {
            BsonValue aVal = a.HasLinks() ? a.GetBsonValue()  : new(0.0);
            BsonValue bVal = b.HasLinks() ? b.GetBsonValue()  : new(0.0);
            Value = Math.Pow(aVal.AsDouble,bVal.AsDouble);
        }
        else
        {
            Value = NodeDefault;
        }
    }
}