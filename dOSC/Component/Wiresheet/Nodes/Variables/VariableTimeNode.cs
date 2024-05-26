using LiveSheet.Parts.Ports;

namespace dOSC.Component.Wiresheet.Nodes.Variables;

public class VariableTimeNode: VariableNode
{
    public VariableTimeNode() : base()
    {
        AddPort(new LiveTimePort(this, false));
    }

    public override string NodeName => "Time Variable";
    public override string Icon => "fa-solid fa-clock";
}