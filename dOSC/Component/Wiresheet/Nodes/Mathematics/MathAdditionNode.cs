using LiteDB;
using LiveSheet;
using LiveSheet.Parts;
using LiveSheet.Parts.Nodes;
using LiveSheet.Parts.Ports;

namespace dOSC.Component.Wiresheet.Nodes.Mathematics;

public class MathAdditionNode : MathNode
{
    public MathAdditionNode() : base()
    {
        AddPort(new LiveNumericPort(this, true, name: "A"));
        AddPort(new LiveNumericPort(this, true, name: "B"));
        AddPort(new LiveNumericPort(this, true, name: "C"));
        AddPort(new LiveNumericPort(this, true, name: "D"));
        AddPort(new LiveNumericPort(this, false, name: "Output"));
    }

    public override string NodeName => "Addition";
    public override string Icon => "fa-solid fa-plus";

    public override void Process(List<EffectedNode>? effectedNodes = null)
    {
        var inputPorts = this.GetInputPorts().Where(x=>x is LiveNumericPort).ToList();
        if (inputPorts.Any() && this.OkToProcess(effectedNodes))
        {
            try
            {
                decimal sum = 0;
                foreach (var port in inputPorts)
                {
                    if (port is LiveNumericPort numericPort)
                    {
                        BsonValue val = numericPort.HasLinks() ? numericPort.GetBsonValue() : new(0.0);
                        if (val != BsonValue.Null)
                        {
                            sum += val.AsDecimal;
                        }
                    }
                }
                Value = sum;
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