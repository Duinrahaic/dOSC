using System.Collections.Concurrent;
using Blazor.Diagrams.Core.Geometry;
using dOSC.Client.Engine.Ports;
using dOSC.Shared.Models.Wiresheet;

namespace dOSC.Client.Engine.Nodes.Utility
{
    public class LogicSwitchNode : BaseNode
    {
        public LogicSwitchNode(Guid? guid = null, ConcurrentDictionary<EntityPropertyEnum, dynamic>? properties = null, Point? position = null) : base(guid, position,properties)
        {
            AddPort(new LogicPort(PortGuids.Port_1, this, true, name: "Switch"));
            AddPort(new MultiPort(PortGuids.Port_2, this, true, name: "Case 1"));
            AddPort(new MultiPort(PortGuids.Port_3, this, true, name: "Case 2"));
            // TO DO: Dynamically add and remove ports
            AddPort(new MultiPort(PortGuids.PortGuidGenerator(1000), this, false, name: "Output"));
            SubscribeToAllPortTypeChanges();
        }
        
        public override string Name => "Logic Switch";
        public override string Category => NodeCategoryType.Utilities;
        public override string Icon => "icon-circuit-board";
        
        public override void CalculateValue()
        {
            var inSwitch = Ports[0];
            var inInputA = Ports[1];
            var inInputB = Ports[2];

            bool? ValA = null!;
            bool? ValB = null!;

            if (inInputA.Links.Any())
            {
                var l1 = inInputA.Links.First();
                try
                {
                    ValA = GetInputValue(inInputA, l1);
                }
                catch
                {
                    ValA = null!;
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
                    ValB = null!;
                }
            }
            if (inSwitch.Links.Any())
            {
                var lSwitch = inSwitch.Links.First();
                bool? SwitchVal = Convert.ToBoolean(GetInputValue(inSwitch, lSwitch));
                if (SwitchVal != null)
                {
                    if (SwitchVal.Value)
                    {
                        Value = ValA;
                    }
                    else
                    {
                        Value = ValB;
                    }
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
