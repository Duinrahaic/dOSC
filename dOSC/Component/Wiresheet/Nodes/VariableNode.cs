using dOSC.Component.Wiresheet.Nodes.Variables;
using LiveSheet.Parts.Serialization;

namespace dOSC.Component.Wiresheet.Nodes;

public abstract class VariableNode: WiresheetNode
{
    public VariableNode() : base()
    {
    }
    
    public override string NodeName => "Variable Node";
    public override NodeCategory Category => NodeCategory.Variables;
    
    [LiveSerialize]
    public bool ExposeToInterface { get; set; } = false;

    public virtual string InputFieldType => "Text";
    
    public virtual int GetMinimumLabelSize() => Value.ToString().Length;
}