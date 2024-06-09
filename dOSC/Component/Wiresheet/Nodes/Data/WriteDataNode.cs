using LiveSheet.Parts.Ports;

namespace dOSC.Component.Wiresheet.Nodes.Data;

public class WriteDataNode : DataNode
{
    public WriteDataNode() : base()
    {
    }
    
    public override string NodeName => "Write Data";
    public override string Icon => "fa-solid fa-arrow-right-to-bracket";
}