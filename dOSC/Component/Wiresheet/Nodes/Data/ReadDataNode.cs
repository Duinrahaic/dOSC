using LiveSheet.Parts.Ports;
using LiveSheet.Parts.Serialization;

namespace dOSC.Component.Wiresheet.Nodes.Data;

public class ReadDataNode: DataNode
{
    public ReadDataNode() : base()
    {
        AddPort(new LiveLogicPort(this, false));
    }
    public override string NodeName => "Read Data";
    public override string Icon => "fa-solid fa-arrow-right-from-bracket";
}