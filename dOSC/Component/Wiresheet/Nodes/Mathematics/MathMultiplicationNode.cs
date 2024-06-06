using LiteDB;
using LiveSheet;
using LiveSheet.Parts;
using LiveSheet.Parts.Nodes;
using LiveSheet.Parts.Ports;

namespace dOSC.Component.Wiresheet.Nodes.Mathematics;

public class MathMultiplicationNode: MathNode
{
    public MathMultiplicationNode() : base()
    {
        AddPort(new LiveNumericPort(this, true, name: "A"));
        AddPort(new LiveNumericPort(this, true, name: "B"));
        AddPort(new LiveNumericPort(this, false, name: "Output"));
    }

    public override string NodeName => "Multiplication";
    public override string Icon => "fa-solid fa-x";
    
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
                Value = aVal * bVal;
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