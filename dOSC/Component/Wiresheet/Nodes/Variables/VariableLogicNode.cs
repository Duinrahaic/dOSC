using LiveSheet.Parts.Ports;

namespace dOSC.Component.Wiresheet.Nodes.Variables;

public class VariableLogicNode: VariableNode
{
    public VariableLogicNode() : base()
    {
        SilentSetValue(false);
        AddPort(new LiveLogicPort(this, false));
    }
    
    public override string NodeName => "Logic Variable";
    public override string Icon => "icon-binary";
    
    private string DefaultTrueText => "True";
    public string TrueText { get; set; } = string.Empty;
    
    private string DefaultFalseText => "False";
    public string FalseText { get; set; } = string.Empty;
    
    public override string GetDisplayValue()
    {
        return Value.AsBoolean ? GetTrueText() : GetFalseText();
    }
    
    public string GetTrueText() => GetAliasOrDefaultText(TrueText, DefaultTrueText);
    public string GetFalseText() => GetAliasOrDefaultText(FalseText, DefaultFalseText);
    
    public override int GetMinimumLabelSize()
    {
        List<string> labels = new List<string> { GetAliasOrDefaultText(TrueText, DefaultTrueText), GetAliasOrDefaultText(FalseText, DefaultFalseText) };
        var size = labels.Max(x => x.Length);
        return size;
    }
    
    private string GetAliasOrDefaultText(string text, string defaultText)
    {
        if(string.IsNullOrWhiteSpace(text))
        {
            return defaultText;
        }
        return text;
    }   
}