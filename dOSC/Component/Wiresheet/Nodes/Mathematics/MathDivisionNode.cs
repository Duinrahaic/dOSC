using LiteDB;
using LiveSheet;
using LiveSheet.Parts;
using LiveSheet.Parts.Nodes;
using LiveSheet.Parts.Ports;

namespace dOSC.Component.Wiresheet.Nodes.Mathematics;

public class MathDivisionNode: MathNode
{
    public MathDivisionNode() : base()
    {
        AddPort(new LiveNumericPort(this, true, name: "Numerator"));
        AddPort(new LiveNumericPort(this, true, name: "Denominator"));
        AddPort(new LiveNumericPort(this, false, name: "Output"));
    }

    public override string NodeName => "Division";
    public override string Icon => "fa-solid fa-divide";
    
    public override void Process(List<EffectedNode>? effectedNodes = null)
    {
        var numeratorPort = Ports[0];
        var denominatorPort = Ports[1];
        if (numeratorPort is LiveNumericPort numerator
            && denominatorPort is LiveNumericPort denominator
            && this.OkToProcess(effectedNodes))
        {
            try
            {
                decimal numeratorVal = numerator.HasLinks() ? numerator.GetBsonValue()  : new(0.0);
                decimal denominatorVal = denominator.HasLinks() ? denominator.GetBsonValue()  : new(1);
                if(denominatorVal == BsonValue.Null && denominatorVal == 0)
                {
                    denominatorVal = 1;
                }
                Value = numeratorVal / denominatorVal;
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