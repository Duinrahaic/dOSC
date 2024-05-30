using LiteDB;

namespace dOSC.Component.Wiresheet.Nodes;

public class MathNode: WiresheetNode
{
    public MathNode() : base()
    {
        this.SilentSetValue(NodeDefault);
    }
    
    public static BsonValue NodeDefault => new(0.0);
    
    public override string NodeName => "Math Node";
    public override NodeCategory Category => NodeCategory.Mathematics;
    
}