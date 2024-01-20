using Blazor.Diagrams.Core.Geometry;
using dOSCEngine.Engine.Ports;
using Newtonsoft.Json;
using System.Collections.Concurrent;

namespace dOSCEngine.Engine.Nodes.Utility
{
    public class NumericSwitchNode : BaseNode
    {
        public NumericSwitchNode(Guid? guid = null, ConcurrentDictionary<EntityPropertyEnum, dynamic>? properties = null, Point? position = null) : base(guid, position, properties)
        {
            AddPort(new NumericPort(PortGuids.Port_1, this, true, name: "Switch"));
            AddPort(new MultiPort(PortGuids.Port_2, this, true, name: "Case 1"));
            AddPort(new MultiPort(PortGuids.Port_3, this, true, name: "Case 2"));
            AddPort(new MultiPort(PortGuids.PortGuidGenerator(1000), this, false, name: "Output"));
            SubscribeToAllPortTypeChanges();
        }
        public override string Name => "Numeric Switch";
        public override string Category => NodeCategoryType.Utilities;
        public override string Icon => "icon-circuit-board";
        public override void CalculateValue()
        {
            var inSwitch = Ports[0];
            var inInputA = Ports[1];
            var inInputB = Ports[2];


            double? ValA = null!;
            double? ValB = null!;

            if (inInputA.Links.Any())
            {
                var l1 = inInputA.Links.First();
                try
                {
                    ValA = GetInputValue(inInputA, l1);
                }
                catch
                {

                }
            }
            if (inInputB.Links.Any())
            {
                var l2 = inInputB.Links.First();
                try
                {
                    ValB = GetInputValue(inInputB, l2);
                }
                catch
                {

                }
            }


            if (inSwitch.Links.Any())
            {
                var lSwitch = inSwitch.Links.First();
                try
                {
                    var SwitchVal = GetInputValue(inSwitch, lSwitch);
                    if (SwitchVal != null)
                    {
                        if (SwitchVal)
                        {
                            Value = ValA;
                        }
                        else
                        {
                            Value = ValB;
                        }
                    }

                }
                catch
                {

                }

            }
            else
            {
                Value = null!;
            }

        }


        public override void OnDispose()
        {
            UnsubscribeToAllPortTypeChanged();
            base.OnDispose();
        }
    }
}
