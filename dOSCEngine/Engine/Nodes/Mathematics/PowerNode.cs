using Blazor.Diagrams.Core.Geometry;
using dOSCEngine.Engine.Ports;
using Newtonsoft.Json;
using System.Collections.Concurrent;

namespace dOSCEngine.Engine.Nodes.Mathematics
{
    public class PowerNode : BaseNode
    {
        public PowerNode(Guid? guid = null, ConcurrentDictionary<EntityPropertyEnum, dynamic>? properties = null, Point? position = null) : base(guid, position, properties)
        {
            AddPort(new NumericPort(PortGuids.Port_1, this, true, name: "Value"));
            AddPort(new NumericPort(PortGuids.Port_2, this, false, name: "Output"));

            Properties.TryInitializeProperty(EntityPropertyEnum.Power, 2);
            _power = Properties.GetProperty<double>(EntityPropertyEnum.Power);
        }
        
        public override string Name => "Power";
        public override string Category => NodeCategoryType.Math;
        public override string Icon => "icon-chevron-up";
        private double _power;
        public override void PropertyNotifyEvent(EntityPropertyEnum property, dynamic? value)
        {
            if(property == EntityPropertyEnum.Power)
            {
                _power = value;
            }
        }
        
        public override void CalculateValue()
        {
            var input = Ports[0];
            if (!input.Links.Any())
            {
                SetValue(null!, false);
            }
            else
            {

                var input_val = GetInputValue(input, input.Links.First());

                if (input == null)
                {
                    SetValue(null!, false);
                }
                else
                {
                    Value = System.Math.Pow(input_val, _power);
                }
            }
        }
    }
}
