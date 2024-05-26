using LiveSheet.Parts.Ports;

namespace dOSC.Component.Wiresheet.Nodes.Variables;

public class VariableStringNode: VariableNode
{
    public VariableStringNode() : base()
    {
        AddPort(new LiveStringPort(this, false));
    }

    public override string NodeName => "String Variable";
    public override string Icon => "fa-solid fa-pen";
}