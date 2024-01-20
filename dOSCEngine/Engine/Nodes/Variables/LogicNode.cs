using System.Collections.Concurrent;
using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using dOSCEngine.Engine.Ports;
using Newtonsoft.Json;

namespace dOSCEngine.Engine.Nodes.Variables
{
    public class LogicNode : BaseNode
    {
        public LogicNode(Guid? guid = null, ConcurrentDictionary<EntityPropertyEnum, dynamic>? properties = null, Point? position = null) : base(guid, position, properties)
        {
            AddPort(new LogicPort(PortGuids.Port_1, this, false, name: "Output"));
            Properties.TryInitializeProperty(EntityPropertyEnum.ConstantValue, false);
            Value = Properties.GetProperty<dynamic>(EntityPropertyEnum.ConstantValue);
            VisualIndicator = Value.ToString();
        }

        public override string Name => "Logic Variable";
        public override string Category => NodeCategoryType.Logic;
        public override string Icon => "icon-binary";
        
        public override void PropertyNotifyEvent(EntityPropertyEnum property, dynamic? value)
        {
            if (property == EntityPropertyEnum.ConstantValue)
            {
                SetValue(value,true);
                VisualIndicator = Value.ToString();
            }
        }
    }
}
