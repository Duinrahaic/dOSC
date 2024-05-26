using LiveSheet.Parts.Ports;

namespace dOSC.Component.Wiresheet.Nodes.Utility;

public class UtilityRandomNode : UtilityNode
{
    public UtilityRandomNode() : base()
    {
        this.SilentSetValue(false);
        
        // Multi Port
        AddPort(new LiveLogicPort(this, false, name: "Output"));
    }
    
    public override string NodeName => "Random Logic";
    public override string Icon => "icon-dices";

    // Recalculate the value of the node
}