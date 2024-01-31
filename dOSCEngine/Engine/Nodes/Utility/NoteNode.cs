using System.Collections.Concurrent;
using Blazor.Diagrams.Core.Geometry;
using Markdig;
using Markdig.Extensions.AutoLinks;
using Markdig.Renderers.Html;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;

namespace dOSCEngine.Engine.Nodes.Utility;

public class NoteNode: BaseNode
{
    public NoteNode(Guid? guid = null, ConcurrentDictionary<EntityProperty, dynamic>? properties = null, Point? position = null) : base(guid, position, properties)
    {
        Properties.TryInitializeProperty(EntityProperty.Note, string.Empty);
        Properties.TryInitializeProperty(EntityProperty.NoteColor, "black");
        
        Note = Properties.GetProperty<string>(EntityProperty.Note);
        NoteColor = Properties.GetProperty<string>(EntityProperty.NoteColor);
    }

    public override void PropertyNotifyEvent(EntityProperty property, dynamic? value)
    {
        if (property == EntityProperty.Note)
        {
            Note = value;
            RequestHeaderUpdate();
        }
        else if(property == EntityProperty.NoteColor)
        {
            NoteColor = value;
            RequestHeaderUpdate();
        }
            
    }
    public string Note;
    public string NoteColor;
    public string GetMarkdownContent()
    {
        var pipeline = new MarkdownPipelineBuilder()
            .UseAdvancedExtensions()
            .Build();
        
        MarkdownDocument document = Markdown.Parse(Note, pipeline);

        return document.ToHtml(pipeline);
    }
        
        
    public override string Name => "Note";
    public override string Category => NodeCategoryType.Utilities;
    public override string Icon => "icon-sticky-note";
}