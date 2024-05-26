using LiteDB;
using LiveSheet;
using LiveSheet.Parts.Nodes;
using LiveSheet.Parts.Ports;

namespace dOSC.Component.Wiresheet.Nodes.Mathematics;

public class MathClampNode : MathNode
{
    public MathClampNode() : base()
    {
        AddPort(new LiveNumericPort(this, true, name: "Input"));
        AddPort(new LiveNumericPort(this, true, name: "Minimum"));
        AddPort(new LiveNumericPort(this, true, name: "Maximum"));
        AddPort(new LiveNumericPort(this, false, name: "Output"));
    }
    public override string NodeName => "Average";
    public override string Icon => "[A,B]";
    public override bool IconIsText => true;

    public override void Process(List<EffectedNode>? effectedNodes = null)
    {
        var inputPort = Ports[0];
        var minPort = Ports[1];
        var maxPort = Ports[2];
        if (inputPort is LiveNumericPort input && minPort is LiveNumericPort min && maxPort is LiveNumericPort max && this.OkToProcess(effectedNodes))
        {
            BsonValue inputVal = input.HasLinks() ? input.GetBsonValue()  : new(0.0);
            BsonValue minVal = min.HasLinks() ? min.GetBsonValue()  : BsonValue.Null;
            BsonValue maxVal = max.HasLinks() ? max.GetBsonValue()  : BsonValue.Null;
            if(minVal == BsonValue.Null && maxVal == BsonValue.Null)
            {
                Value = inputVal;
            }
            else if(minVal == BsonValue.Null)
            {
                Value = inputVal > maxVal ? maxVal : inputVal;
            }
            else if(maxVal == BsonValue.Null)
            {
                Value = inputVal < minVal ? minVal : inputVal;
            }
            else
            {
                var temp  = inputVal < minVal ? minVal : inputVal;
                Value = temp > maxVal ? maxVal : temp;
            }
        }
        else
        {
            Value = NodeDefault;
        }
    }
}