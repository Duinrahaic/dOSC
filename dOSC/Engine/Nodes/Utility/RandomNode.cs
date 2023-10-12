using Blazor.Diagrams.Core.Geometry;
using dOSC.Engine.Ports;

namespace dOSC.Engine.Nodes.Utility
{
    public class RandomNode : BaseNode
    {
        public RandomNode(Point? position = null) : base(position ?? new Point(0, 0))
        {
            AddPort(new NumericPort(this, false));
        }
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
