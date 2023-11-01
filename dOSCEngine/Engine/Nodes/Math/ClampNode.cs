using Blazor.Diagrams.Core.Geometry;
using dOSCEngine.Engine.Ports;
using Newtonsoft.Json;

namespace dOSCEngine.Engine.Nodes.Math
{
    public class ClampNode : BaseNode
    {
        public ClampNode(Point? position = null) : base(position ?? new Point(0, 0))
        {
            AddPort(new NumericPort(PortGuids.Port_1, this, true));
            AddPort(new NumericPort(PortGuids.Port_2, this, true));
            AddPort(new NumericPort(PortGuids.Port_3, this, true));
            AddPort(new NumericPort(PortGuids.Port_3, this, false));
        }
        public ClampNode(Guid guid, Point? position = null) : base(guid, position ?? new Point(0, 0))
        {
            AddPort(new NumericPort(PortGuids.Port_1, this, true));
            AddPort(new NumericPort(PortGuids.Port_2, this, true));
            AddPort(new NumericPort(PortGuids.Port_3, this, true));
            AddPort(new NumericPort(PortGuids.Port_4, this, false));
        }
        [JsonProperty]
        public override string NodeClass => GetType().Name.ToString();
        public override string BlockTypeClass => "numericblock";


        public override void CalculateValue()
        {
            var input = Ports[0];
            var min = Ports[1];
            var max = Ports[2];
            var value = Ports[3];
            if (!input.Links.Any())
            {
                Value = null;
            }
            else
            {
                if(!input.Links.Any() || !min.Links.Any() || !max.Links.Any())
                {
                    return;
                }
                var input_val = GetInputValue(input, input.Links.First());
                var min_val = GetInputValue(min, min.Links.First());
                var max_val = GetInputValue(max, max.Links.First());
                if (input_val == null || min_val == null || max_val == null)
                {
                    Value = null;
                }
                else
                {
                    if(min_val > max_val)
                    {
                        var temp = min_val;
                        min_val = max_val;
                        max_val = temp;
                    }

                    Value = System.Math.Clamp(input_val, min_val, max_val);


                }
            }
        }
    }
}
