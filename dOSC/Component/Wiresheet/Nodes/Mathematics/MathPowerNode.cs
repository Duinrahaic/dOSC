using LiteDB;
using LiveSheet;
using LiveSheet.Parts;
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
            try
            {
                decimal aVal = a.HasLinks() ? a.GetBsonValue()  : new(0.0);
                decimal bVal = b.HasLinks() ? b.GetBsonValue()  : new(0.0);
                Value = DecimalPow(aVal,bVal);
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
    
    private static decimal DecimalPow(decimal baseValue, decimal exponent)
    {
        if (baseValue == 0 && exponent == 0)
            throw new ArgumentException("0^0 is undefined.");
        
        if (baseValue < 0 && exponent % 1 != 0)
            throw new ArgumentException("Negative base with non-integer exponent is undefined in real numbers.");
        
        double baseDouble = (double)baseValue;
        double exponentDouble = (double)exponent;

        // Using Math.Exp and Math.Log to calculate the power
        double resultDouble = Math.Exp(exponentDouble * Math.Log(baseDouble));
        return (decimal)resultDouble;
    }
}