using LiteDB;
using LiveSheet;
using LiveSheet.Parts.Nodes;
using LiveSheet.Parts.Ports;

namespace dOSC.Component.Wiresheet.Nodes.Logic;

public class LogicNotNode: LogicNode
{
    public LogicNotNode() : base()
    {
        AddPort(new LiveLogicPort(this, true, name: "A"));
        AddPort(new LiveLogicPort(this, false, name: "!A"));
    }
    public override string NodeName => "Not";
    public override bool IconIsText => true;
    public override string Icon => "!A";
    public override void Process(List<EffectedNode>? effectedNodes = null)
    {
        var inA = Ports[0];
        if(inA is LiveLogicPort a && this.OkToProcess(effectedNodes))
        {
            BsonValue aVal = a.HasLinks() ? a.GetBsonValue() : false;
            
            Value = LogicOperations.NotOperation(aVal);
        }
        else
        {
            Value = false;
        }
    }
}