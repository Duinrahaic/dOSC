using System.Collections.Concurrent;
using Blazor.Diagrams.Core.Geometry;
using dOSCEngine.Engine.Ports;
using Newtonsoft.Json;

namespace dOSCEngine.Engine.Nodes.Variables;
public class NumericNode : BaseNode
{
    public NumericNode(Guid? guid = null, ConcurrentDictionary<EntityPropertyEnum, dynamic>? properties = null, Point? position = null) : base(guid, position, properties)
    {
        AddPort(new NumericPort(PortGuids.Port_1, this, false, name: "Output"));
        Properties.TryInitializeProperty(EntityPropertyEnum.ConstantValue, 0.0);
        Value = Properties.GetProperty<dynamic>(EntityPropertyEnum.ConstantValue);
        VisualIndicator = Value.ToString("G5");
    }
    public override string Name => "Numeric Variable";
    public override string Category => NodeCategoryType.Math;
    public override string TextIcon => "#";

    public override void PropertyNotifyEvent(EntityPropertyEnum property, dynamic? value)
    {
        if (property == EntityPropertyEnum.ConstantValue)
        {
            SetValue(value,true);
            VisualIndicator = Value.ToString("G5");
        }
    }
}

