using Blazor.Diagrams.Core.Geometry;
using dOSCEngine.Engine.Ports;
using dOSCEngine.Utilities;
using Newtonsoft.Json;
using System.Collections.Concurrent;

namespace dOSCEngine.Engine.Nodes.Mathematics
{
    public class AverageNode : BaseNode
    {
        public AverageNode(Guid? guid = null, ConcurrentDictionary<EntityProperty, dynamic>? properties = null, Point? position = null) : base(guid, position, properties)
        {
            AddPort(new NumericPort(PortGuids.Port_1, this, true, name: "Inputs", false));
            AddPort(new NumericPort(PortGuids.Port_2, this, false, name: "Average"));

            Properties.TryInitializeProperty(EntityProperty.MaxQueue, 1);
            QueueSize = Properties.GetProperty<int>(EntityProperty.MaxQueue);
        }

        private ShiftedList<double> _history = new();
        public override string Name => QueueSize >= 1 ? "Rolling Average" : "Average";
        public override string Category => NodeCategoryType.Math;
        public override string TextIcon => "x̄";
        public override void PropertyNotifyEvent(EntityProperty property, dynamic? value)
        {
            if (property == EntityProperty.MaxQueue)
            {
                QueueSize = value;
                if (QueueSize > 1)
                {
                    _history.Resize(QueueSize);
                }
                else
                {
                    _history.Clear();
                }
            }
        }
        public override void CalculateValue()
        {
            var inputs = Ports[0];
            if (!inputs.Links.Any())
                return;

            List<double> values = new List<double>();

            foreach (var link in inputs.Links)
            {
                var val = GetInputValue(inputs, link);
                if(val != null)
                    values.Add(val);
            }
            
            if (QueueSize == 1)
            {
                if(values.Any())
                {
                    Value = values.Average();
                }
            }
            else if (QueueSize > 1)
            {
                if (values.Any())
                {
                    _history.ShiftLeft(1);
                    _history.Add(values.Average());
                }
                if (_history.GetCopy().Any())
                {
                    Value = _history.GetCopy().Average();
                }
            }
            else
            {
                SetValue(null!, false);
            }

             
        }

    }
}
