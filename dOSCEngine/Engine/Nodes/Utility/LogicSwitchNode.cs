using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using dOSCEngine.Engine.Ports;
using Newtonsoft.Json;

namespace dOSCEngine.Engine.Nodes.Utility
{
    public class LogicSwitchNode : BaseNode
    {
        public LogicSwitchNode(Point? position = null) : base(position ?? new Point(0, 0))
        {
            AddPort(new LogicPort(PortGuids.Port_1, this, true));
            AddPort(new LogicPort(PortGuids.Port_2, this, true));
            AddPort(new LogicPort(PortGuids.Port_3, this, true));
            AddPort(new LogicPort(PortGuids.Port_4, this, false));
        }
        public LogicSwitchNode(Guid guid, Point? position = null) : base(guid, position ?? new Point(0, 0))
        {
            AddPort(new LogicPort(PortGuids.Port_1, this, true));
            AddPort(new LogicPort(PortGuids.Port_2, this, true));
            AddPort(new LogicPort(PortGuids.Port_3, this, true));
            AddPort(new LogicPort(PortGuids.Port_4, this, false));
        }

        [JsonProperty]
        public override string NodeClass => GetType().Name.ToString();
        public override string BlockTypeClass => "logicblock";


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
                bool? SwitchVal = GetInputValue(inSwitch, lSwitch);
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



    }
}
