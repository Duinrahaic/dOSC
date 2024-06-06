using LiteDB;
using LiveSheet;
using LiveSheet.Parts;
using LiveSheet.Parts.Nodes;
using LiveSheet.Parts.Ports;

namespace dOSC.Component.Wiresheet.Nodes.Logic;

public class LogicXOrNode : LogicNode
{
    public LogicXOrNode() : base()
    {
        AddPort(new LiveLogicPort(this, true, name: "A"));
        AddPort(new LiveLogicPort(this, true, name: "B"));
        AddPort(new LiveLogicPort(this, false, name: "Output"));
    }
    public override string NodeName => "XOR";
    public override string Icon => "fa-solid fa-angle-up";
    public override void Process(List<EffectedNode>? effectedNodes = null)
    {
        var inA = Ports[0];
        var inB = Ports[1];
        if(inA is LiveLogicPort a && inB is LiveLogicPort b && OkToProcess(effectedNodes))
        {
            try
            {
                BsonValue aVal = a.HasLinks() ? a.GetBsonValue() : false;
                BsonValue bVal = b.HasLinks() ? b.GetBsonValue() : false;
            
                Value = LogicOperations.XorOperation(aVal, bVal);
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
            ClearErrorMessage();
        }
    }
}