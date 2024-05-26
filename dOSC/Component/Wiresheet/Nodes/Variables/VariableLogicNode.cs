using LiveSheet.Parts.Ports;

namespace dOSC.Component.Wiresheet.Nodes.Variables;

public class VariableLogicNode: VariableNode
{
    public VariableLogicNode() : base()
    {
        AddPort(new LiveLogicPort(this, false));
    }
    
    public override string NodeName => "Logic Variable";
    public override string Icon => "icon-binary";
    
}