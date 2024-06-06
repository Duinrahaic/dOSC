using dOSC.Component.Wiresheet.Nodes.Logic;
using LiteDB;
using LiveSheet;
using LiveSheet.Parts;
using LiveSheet.Parts.Nodes;
using LiveSheet.Parts.Ports;

namespace dOSC.Component.Wiresheet.Nodes.Mathematics;

public class MathAbsoluteNode : MathNode
{
    public MathAbsoluteNode() : base()
    {
        AddPort(new LiveNumericPort(this, true, name: "A"));
        AddPort(new LiveNumericPort(this, false, name: "|A|"));
    }

    public override string NodeName => "Absolute";
    public override string Icon => "|x|";
    public override bool IconIsText => true;

    public override void Process(List<EffectedNode>? effectedNodes = null)
    {
        var inA = Ports[0];
        if (inA is LiveNumericPort a && this.OkToProcess(effectedNodes))
        {
            
            try
            {
                BsonValue aVal = a.HasLinks() ? a.GetBsonValue()  : new(0.0);
                Value = Math.Abs(aVal.AsDecimal);
                ClearErrorMessage();
            }
            catch
            {
                SetErrorMessage(LiveErrorMessages.FailedToCalculate);
            }
            
        }
        else
        {
            Value = NodeDefault;
            ClearErrorMessage();
        }
    }
}