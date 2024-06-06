using LiveSheet.Parts.Ports;
using LiveSheet.Parts.Serialization;

namespace dOSC.Component.Wiresheet.Nodes.Variables;

public class VariableNumericNode : VariableNode
{
    public VariableNumericNode() : base()
    {
        SilentSetValue(0);
        AddPort(new LiveNumericPort(this, false));
    }

    public override string NodeName => "Numeric Variable";
    public override string Icon => "fa-solid fa-hashtag";

    public int GetMinimumLabelSize() => Value.AsDecimal.ToString().Length;
    [LiveSerialize] public int DecimalPlaces { get; set; } = 0;
    [LiveSerialize] public decimal MaxValue { get; set; } = decimal.MaxValue;
    [LiveSerialize] public decimal MinValue { get; set; } = decimal.MinValue;
    [LiveSerialize] public decimal Step { get; set; } = 1;
    
    
}
