using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using dOSCEngine.Engine.Nodes;
using dOSCEngine.Engine.Ports;
using Newtonsoft.Json;

namespace dOSCEngine.Engine.Nodes.Math
{
    public class SummationNode : BaseNode
    {
        public SummationNode(Point? position = null) : base(position ?? new Point(0, 0))
        {
            AddPort(new NumericPort(PortGuids.Port_1, this, true, false));
            AddPort(new NumericPort(PortGuids.Port_2, this, false));
        }
        public SummationNode(Guid guid, Point? position = null) : base(guid, position ?? new Point(0, 0))
        {
            AddPort(new NumericPort(PortGuids.Port_1, this, true, false));
            AddPort(new NumericPort(PortGuids.Port_2, this, false));
        }
        [JsonProperty]
        public override string NodeClass => GetType().Name.ToString();

        public override string BlockTypeClass => "numericblock";


        public override void CalculateValue()
        {
            var inputs = Ports[0];

            var sum = 0.0;

            foreach (var link in inputs.Links)
            {
                var val = GetInputValue(inputs, link);
                if (val != null)
                {
                    sum += val;
                }
            }
            Value = sum;
        }

    }
}
