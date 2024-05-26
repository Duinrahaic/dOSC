using LiveSheet.Parts.Nodes;

namespace dOSC.Component.Wiresheet.Nodes.Logic;

public class LogicNode: WiresheetNode
{
    public LogicNode() : base()
    {
        this.SilentSetValue(false);
    }
    
    public override string NodeName => "Logic Node";
    public override NodeCategory Category => NodeCategory.Logic;
    
}