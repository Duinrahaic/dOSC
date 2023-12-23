using Blazor.Diagrams.Core.Geometry;
using dOSCEngine.Engine.Ports;
using Newtonsoft.Json;

namespace dOSCEngine.Engine.Nodes.Utility
{
    public class RandomNode : BaseNode
    {
        public RandomNode(Point? position = null) : base(position ?? new Point(0, 0))
        {
            AddPort(new NumericPort(PortGuids.Port_1, this, false));
        }
        public RandomNode(Guid guid, Point? position = null) : base(guid, position ?? new Point(0, 0))
        {
            AddPort(new NumericPort(PortGuids.Port_1, this, false));
        }
        [JsonProperty]
        public override string NodeClass => GetType().Name.ToString();
        public override string BlockTypeClass => "numericblock";
        private Random Random = new Random();
        public override void CalculateValue()
        {
            var port = Ports[0];
            if (port != null)
            {
                if (port.Links.Any())
                {
                    Value = Random.NextDouble();
                }
            }
        }

    }
}
