using System.Collections.Concurrent;
using Blazor.Diagrams.Core.Geometry;
using dOSCEngine.Engine.Ports;
using Newtonsoft.Json;

namespace dOSCEngine.Engine.Nodes.Variables;
public class NumericNode : BaseNode
{
    public NumericNode(Guid? guid = null, ConcurrentDictionary<EntityPropertyEnum, dynamic>? properties = null, Point? position = null) : base(guid, position, properties)
    {
        Port = new NumericPort(PortGuids.Port_1, this, false, name: "Output");
        AddPort(Port);
        Properties.TryInitializeProperty(EntityPropertyEnum.ConstantValue, 0.0);
        Value = Properties.GetProperty<dynamic>(EntityPropertyEnum.ConstantValue);
        VisualIndicator = Value.ToString("G5");
        Port.OnPortLinksChanged += SendValue;
    }
    public override string Name => "Numeric Variable";
    public override string Category => NodeCategoryType.Math;
    public override string TextIcon => "#";
    private NumericPort Port { get; set; }

    public override void PropertyNotifyEvent(EntityPropertyEnum property, dynamic? value)
    {
        if (property == EntityPropertyEnum.ConstantValue)
        {
            SetValue(value,true);
            VisualIndicator = Value.ToString("G5");
        }
    }
    
    private void SendValue(BasePort port)
    {
        if (Port.HasValidLinks())
        {
            SetValue(Value,true);
        }
    }

    public override void OnDispose()
    {
        Port.OnPortLinksChanged -= SendValue;
        base.OnDispose();
    }
}

