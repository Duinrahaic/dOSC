using LiveSheet.Parts.Ports;

namespace dOSC.Component.Wiresheet.Nodes.Data;

public class WriteDataNode : DataNode
{
    public WriteDataNode() : base()
    {
        AddPort(new LiveLogicPort(this, true));
    }
    
    public override string NodeName => "Write Data";
    public override string Icon => "fa-solid fa-arrow-right-to-bracket";
}