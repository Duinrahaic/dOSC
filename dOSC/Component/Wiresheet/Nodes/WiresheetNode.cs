using LiveSheet.Parts.Nodes;
using LiveSheet.Parts.Serialization;

namespace dOSC.Component.Wiresheet.Nodes;

public class WiresheetNode: LiveNode
{
    public WiresheetNode() : base(new(0,0))
    {
    }

    public override string NodeName => "WireSheet Node";
    
    public virtual NodeCategory Category { get; private set; } = Nodes.NodeCategory.Data;
    public virtual string Icon { get; private set; } = "fa-solid fa-square";
    public virtual bool IconIsText { get; private set; } = false;
    
    public virtual bool IsWritable => false;
}