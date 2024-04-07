using System.Collections.Concurrent;
using Blazor.Diagrams.Core.Geometry;
using dOSC.Shared.Models.Wiresheet;
using Markdig;

namespace dOSC.Client.Engine.Nodes.Utility;

public class NoteNode : BaseNode
{
    public string Note;
    public string NoteColor;

    public NoteNode(Guid? guid = null, ConcurrentDictionary<EntityPropertyEnum, dynamic>? properties = null,
        Point? position = null) : base(guid, position, properties)
    {
        Properties.TryInitializeProperty(EntityPropertyEnum.Note, string.Empty);
        Properties.TryInitializeProperty(EntityPropertyEnum.NoteColor, "black");

        Note = Properties.GetProperty<string>(EntityPropertyEnum.Note);
        NoteColor = Properties.GetProperty<string>(EntityPropertyEnum.NoteColor);
    }

    public string MarkdownContent => Markdown.ToHtml(Note);
    public override string Name => "Note";
    public override string Category => NodeCategoryType.Utilities;
    public override string Icon => "icon-sticky-note";

    public override void PropertyNotifyEvent(EntityPropertyEnum property, dynamic? value)
    {
        if (property == EntityPropertyEnum.Note)
        {
            Note = value;
            RequestHeaderUpdate();
        }
        else if (property == EntityPropertyEnum.NoteColor)
        {
            NoteColor = value;
            RequestHeaderUpdate();
        }
    }
}