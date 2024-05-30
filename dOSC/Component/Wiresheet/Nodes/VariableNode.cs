using LiveSheet.Parts.Serialization;

namespace dOSC.Component.Wiresheet.Nodes;

public class VariableNode: WiresheetNode
{
    public VariableNode() : base()
    {
        this.SilentSetValue(0.0);
    }
    
    public override string NodeName => "Variable Node";
    public override NodeCategory Category => NodeCategory.Variables;
    
    [LiveSerialize]
    public bool ExposeToInterface { get; set; } = false;
}