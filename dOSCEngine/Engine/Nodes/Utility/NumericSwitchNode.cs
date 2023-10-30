using Blazor.Diagrams.Core.Geometry;
using dOSCEngine.Engine.Ports;
using Newtonsoft.Json;

namespace dOSCEngine.Engine.Nodes.Utility
{
    public class NumericSwitchNode : BaseNode
    {
        public NumericSwitchNode(Point? position = null) : base(position ?? new Point(0, 0))
        {
            AddPort(new LogicPort(PortGuids.Port_1, this, true));
            AddPort(new NumericPort(PortGuids.Port_2, this, true));
            AddPort(new NumericPort(PortGuids.Port_3, this, true));
            AddPort(new NumericPort(PortGuids.Port_4, this, false));
        }
        public NumericSwitchNode(Guid guid, Point? position = null) : base(guid, position ?? new Point(0, 0))
        {
            AddPort(new LogicPort(PortGuids.Port_1, this, true));
            AddPort(new NumericPort(PortGuids.Port_2, this, true));
            AddPort(new NumericPort(PortGuids.Port_3, this, true));
            AddPort(new NumericPort(PortGuids.Port_4, this, false));
        }

        [JsonProperty]
        public override string NodeClass => GetType().Name.ToString();
        public override string BlockTypeClass => "numericblock";


        public override void Refresh()
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
                    if(SwitchVal != null)
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

            base.Refresh();
        }



    }
}
