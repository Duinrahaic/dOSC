using Blazor.Diagrams.Core.Geometry;
using dOSCEngine.Engine.Ports;
using Newtonsoft.Json;

namespace dOSCEngine.Engine.Nodes.Math
{
    public class NegativeNode : BaseNode
    {
        public NegativeNode(Point? position = null) : base(position ?? new Point(0, 0))
        {
            AddPort(new NumericPort(PortGuids.Port_1, this, true));
            AddPort(new NumericPort(PortGuids.Port_2, this, false));
        }
        public NegativeNode(Guid guid, Point? position = null) : base(guid, position ?? new Point(0, 0))
        {
            AddPort(new NumericPort(PortGuids.Port_1, this, true));
            AddPort(new NumericPort(PortGuids.Port_2, this, false));
        }
        [JsonProperty]
        public override string NodeClass => GetType().Name.ToString();
        public override string BlockTypeClass => "numericblock";


        public override void CalculateValue()
        {
            var inputs = Ports[0];
            if (!inputs.Links.Any())
            {
                Value = null;
            }
            else
            {
                List<double> values = new List<double>();
                inputs.Links.Select(x => GetInputValue(inputs, x)).Where(x => x != null).Select(x => values.Add(x));
                if (values.Any())
                {
                    Value = -1 * values.First();
                }
                else
                {
                    Value = null;
                }
            }
        }

    }
}
