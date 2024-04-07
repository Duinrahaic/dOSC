using System;
using System.Collections.Concurrent;
using Blazor.Diagrams.Core.Geometry;

namespace dOSC.Shared.Models.Wiresheet;

public class BaseNodeDTO
{
    public BaseNodeDTO(Guid guid, string nodeClass, dynamic value,
        ConcurrentDictionary<EntityPropertyEnum, dynamic> properties, Point position)
    {
        Guid = guid;
        NodeClass = nodeClass;
        Value = value;
        Position = position ?? new Point(0, 0);
        Properties = properties;
    }

    public BaseNodeDTO(Guid guid, string nodeClass, dynamic value, string option,
        ConcurrentDictionary<EntityPropertyEnum, dynamic> properties, Point position)
    {
        Guid = guid;
        NodeClass = nodeClass;
        Value = value;
        Option = string.IsNullOrEmpty(option) ? string.Empty : option;
        Position = position ?? new Point(0, 0);
        Properties = properties;
    }

    public BaseNodeDTO()
    {
    }

    public Guid Guid { get; set; }
    public string NodeClass { get; set; }
    public dynamic Value { get; set; }
    public string Option { get; set; } = string.Empty;
    public Point Position { get; set; } = new(0, 0);
    public ConcurrentDictionary<EntityPropertyEnum, dynamic> Properties { get; set; } = new();
}