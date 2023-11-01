using Blazor.Diagrams.Core.Geometry;
using dOSCEngine.Engine.Ports;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dOSCEngine.Engine.Nodes.Math
{
    public class RollingAverageNode : BaseNode
    {
        public RollingAverageNode(Point? position = null) : base(position ?? new Point(0, 0))
        {
            AddPort(new NumericPort(PortGuids.Port_1, this, true));
            AddPort(new NumericPort(PortGuids.Port_2, this, false));
        }
        public RollingAverageNode(Guid guid, Point? position = null) : base(guid, position ?? new Point(0, 0))
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
            if (!inputs.Links.Any())
            {
                Value = null;
            }
            else
            {

                var firstLink = inputs.Links.First();
                var original = Value;
                var New = GetInputValue(inputs, firstLink);

                if (New != null)
                {
                    if (original == null)
                    {
                        Value = New;
                    }
                    else
                    {
                        if (New == original)
                            return;
                        var calc = ((original + New) / 2);
                        Value = calc;
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
