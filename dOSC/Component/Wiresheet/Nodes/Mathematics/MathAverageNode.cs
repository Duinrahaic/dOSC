using LiteDB;
using LiveSheet;
using LiveSheet.Parts.Nodes;
using LiveSheet.Parts.Ports;

namespace dOSC.Component.Wiresheet.Nodes.Mathematics;

public class MathAverageNode: MathNode
{
    public MathAverageNode() : base()
    {
        AddPort(new LiveNumericPort(this, true, name: "A"));
        AddPort(new LiveNumericPort(this, true, name: "B"));
        AddPort(new LiveNumericPort(this, true, name: "C"));
        AddPort(new LiveNumericPort(this, true, name: "D"));
        AddPort(new LiveNumericPort(this, false, name: "Output"));
    }

    public override string NodeName => "Average";
    public override string Icon => "μ";
    public override bool IconIsText => true;

    public override void Process(List<EffectedNode>? effectedNodes = null)
    {
        var inputPorts = this.GetInputPorts().Where(x=>x is LiveNumericPort).ToList();
        if (inputPorts.Any() && this.OkToProcess(effectedNodes))
        {
            double sum = 0.0;
            double count = 0.0;
            foreach (var port in inputPorts)
            {
                if (port is LiveNumericPort numericPort)
                {
                    BsonValue val = numericPort.HasLinks() ? numericPort.GetBsonValue() : new(0.0);
                    if (val != BsonValue.Null && numericPort.HasLinks())
                    {
                        sum += val.AsDouble;
                        count++;
                    }
                }
            }
            Value = sum / count == 0 ? 1 : count;
        }
        else
        {
            Value = NodeDefault;
        }
    }
}