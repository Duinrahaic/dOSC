using Blazor.Diagrams.Core.Geometry;
using dOSC.Engine.Ports;
using Newtonsoft.Json;

namespace dOSC.Engine.Nodes.Utility
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
        public override string NodeClass => this.GetType().Name.ToString();
        public override string BlockTypeClass => "numericblock";
        private Random Random = new Random();
        public override void Refresh()
        {
            var port = Ports[0];
            if(port != null)
            {
                if (port.Links.Any())
                {
                    Value = Random.NextDouble();
                }
            }
            base.Refresh();
        }

    }
}
