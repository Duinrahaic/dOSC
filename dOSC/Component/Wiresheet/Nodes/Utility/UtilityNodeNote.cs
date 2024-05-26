using LiveSheet.Parts.Serialization;
using Markdig;

namespace dOSC.Component.Wiresheet.Nodes.Utility;

public class UtilityNodeNote: UtilityNode
{
    
    [LiveSerialize]
    public string Note { get; set; } = String.Empty;
    [LiveSerialize]
    public NoteColor NoteColor { get; set; } = NoteColor.Black;

    public override string NodeName => "Note";
    public override string Icon => "icon-sticky-note";
    
    public string MarkdownContent => Markdown.ToHtml(Note);
    
    

}