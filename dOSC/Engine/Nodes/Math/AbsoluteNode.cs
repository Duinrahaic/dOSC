using Blazor.Diagrams.Core.Geometry;
using dOSC.Engine.Ports;
using Newtonsoft.Json;

namespace dOSC.Engine.Nodes.Math
{
    public class AbsoluteNode : BaseNode
    {
        public AbsoluteNode(Point? position = null) : base(position ?? new Point(0, 0))
        {
            AddPort(new NumericPort(PortGuids.Port_1, this, true));
            AddPort(new NumericPort(PortGuids.Port_2, this, false));
        }
        public AbsoluteNode(Guid guid, Point? position = null) : base(guid, position ?? new Point(0, 0))
        {
            AddPort(new NumericPort(PortGuids.Port_1, this, true));
            AddPort(new NumericPort(PortGuids.Port_2, this, false));
        }
        [JsonProperty]
        public override string NodeClass => GetType().Name.ToString();
        public override string BlockTypeClass => "numericblock";


        public override void Refresh()
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
                    Value = System.Math.Abs(values.First());
                }
                else
                {
                    Value = null;
                }
            }
            base.Refresh();
        }

    }
}
