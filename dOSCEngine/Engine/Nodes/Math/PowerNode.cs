using Blazor.Diagrams.Core.Geometry;
using dOSCEngine.Engine.Nodes;
using dOSCEngine.Engine.Ports;
using Newtonsoft.Json;

namespace dOSCEngine.Engine.Nodes.Math
{
    public class PowerNode : BaseNode
    {
        public PowerNode(Point? position = null) : base(position ?? new Point(0, 0))
        {
            AddPort(new NumericPort(PortGuids.Port_1, this, true));
            AddPort(new NumericPort(PortGuids.Port_2, this, true));
            AddPort(new NumericPort(PortGuids.Port_3, this, false));
        }
        public PowerNode(Guid guid, Point? position = null) : base(guid, position ?? new Point(0, 0))
        {
            AddPort(new NumericPort(PortGuids.Port_1, this, true));
            AddPort(new NumericPort(PortGuids.Port_2, this, true));
            AddPort(new NumericPort(PortGuids.Port_3, this, false));
        }
        [JsonProperty]
        public override string NodeClass => GetType().Name.ToString();
        public override string BlockTypeClass => "numericblock";


        public override void CalculateValue()
        {
            var inputs = Ports[0];
            var power = Ports[1];
            if (!inputs.Links.Any())
            {
                Value = null;
            }
            else
            {

                if (inputs.Links.Any())
                {
                    double power_of = 2.0;
                    if (power.Links.Any())
                    {
                        power_of = GetInputValue(power, power.Links.First());
                        if (double.IsNaN(power_of))
                        {
                            power_of = 2.0;
                        }
                    }

                    var input_val = GetInputValue(inputs, inputs.Links.First());

                    if (double.IsNaN(input_val))
                    {
                        Value = null;
                    }
                    else
                    {
                        Value = System.Math.Pow(input_val, power_of);
                    }
                }
                else
                {
                    Value = null;
                }
            }
        }

    }
}
