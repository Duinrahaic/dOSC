using Blazor.Diagrams.Core.Geometry;

namespace dOSCEngine.Engine.Nodes
{
    public class BaseNodeDTO
    {
        public Guid Guid { get; set; }
        public string NodeClass { get; set; }
        public dynamic Value { get; set; }
        public string Option { get; set; } = string.Empty;
        public Point Position { get; set; } = new Point(0, 0);
        public BaseNodeDTO(BaseNode node)
        {

            Guid = node.Guid;
            NodeClass = node.NodeClass;
            Value = node.Value;
            Position = node.Position;
            Option = node.Option;
        }

        public BaseNodeDTO(Guid guid, string nodeClass, dynamic value, Point position)
        {
            Guid = guid;
            NodeClass = nodeClass;
            Value = value;
            Position = position ?? new Point(0, 0);
        }

        public BaseNodeDTO(Guid guid, string nodeClass, dynamic value, string option, Point position)
        {
            Guid = guid;
            NodeClass = nodeClass;
            Value = value;
            Option = string.IsNullOrEmpty(option) ? string.Empty : option;
            Position = position ?? new Point(0, 0);
        }

        public BaseNodeDTO() { }

    }
}
