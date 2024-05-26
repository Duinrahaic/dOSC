using LiveSheet.Parts.Ports;

namespace dOSC.Component.Wiresheet.Nodes.Variables;

public class VariableNumericNode : VariableNode
{
    public VariableNumericNode() : base()
    {
        AddPort(new LiveNumericPort(this, false));
    }

    public override string NodeName => "Numeric Variable";
    public override string Icon => "fa-solid fa-hashtag";
}
