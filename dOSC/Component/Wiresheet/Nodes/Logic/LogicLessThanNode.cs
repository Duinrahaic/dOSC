using LiteDB;
using LiveSheet;
using LiveSheet.Parts;
using LiveSheet.Parts.Nodes;
using LiveSheet.Parts.Ports;

namespace dOSC.Component.Wiresheet.Nodes.Logic;

public class LogicLessThanNode: LogicNode
{
    public LogicLessThanNode() : base()
    {
        AddPort(new LiveNumericPort(this, true, name: "A"));
        AddPort(new LiveNumericPort(this, true, name: "B"));
        AddPort(new LiveLogicPort(this, false, name: "Output"));
    }

    public override string NodeName => "Less Than";
    public override string Icon => "fa-solid fa-less-than";
    public override void Process(List<EffectedNode>? effectedNodes = null)
    {
        var inA = Ports[0];
        var inB = Ports[1];
        if (inA is LiveNumericPort a && inB is LiveNumericPort b && this.OkToProcess(effectedNodes))
        {
            try
            {
                BsonValue aVal = a.HasLinks() ? a.GetBsonValue()  : new(0.0);
                BsonValue bVal = b.HasLinks() ? b.GetBsonValue()  : new(0.0);

                Value = LogicOperations.LessThan(aVal, bVal);
                ClearErrorMessage();
            }
            catch
            {
                SetErrorMessage(LiveErrorMessages.BadConfiguration);
            }
        }
        else
        {
            Value = false;
        }
    }
}