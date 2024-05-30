using dOSC.Component.Wiresheet.Nodes.Data;
using LiveSheet.Parts.Ports;
using LiveSheet.Parts.Serialization;

namespace dOSC.Component.Wiresheet.Nodes;

public class DataNode: WiresheetNode
{
    public DataNode() : base()
    {
        this.SilentSetValue(0.0);
    }
    
    public override string NodeName => "Data Node";
    public override NodeCategory Category => NodeCategory.Data;
    
    [LiveSerialize] public string Source { get; set; } = "";
    [LiveSerialize] public string Address { get; set; } = "";
    [LiveSerialize] public PortType PortType { get; set; } = PortType.Logic;
    [LiveSerialize] public DataType DataType { get; set; } = DataType.Boolean;
}