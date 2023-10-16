using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using dOSC.Engine.Ports;
using Newtonsoft.Json;

namespace dOSC.Engine.Nodes.Utility
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
        public override string NodeClass => this.GetType().Name.ToString();

        public override string BlockTypeClass => "numericblock";


        public override void Refresh()
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
            base.Refresh();
        }

    }
}
