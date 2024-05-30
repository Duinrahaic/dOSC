namespace dOSC.Component.Wiresheet.Nodes;

public class UtilityNode: WiresheetNode
{
    public UtilityNode() : base()
    {
        this.SilentSetValue(0.0);
    }
    
    public override string NodeName => "Utility Node";
    public override NodeCategory Category => NodeCategory.Utility;
    
}