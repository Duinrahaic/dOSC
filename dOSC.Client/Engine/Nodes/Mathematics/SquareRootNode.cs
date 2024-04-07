using System.Collections.Concurrent;
using Blazor.Diagrams.Core.Geometry;
using dOSC.Client.Engine.Ports;
using dOSC.Shared.Models.Wiresheet;

namespace dOSC.Client.Engine.Nodes.Mathematics
{
    public class SquareRootNode : BaseNode
    {
        public SquareRootNode(Guid? guid = null, ConcurrentDictionary<EntityPropertyEnum, dynamic>? properties = null, Point? position = null) : base(guid, position, properties)
        {
            AddPort(new NumericPort(PortGuids.Port_1, this, true, name: "Value"));
            AddPort(new NumericPort(PortGuids.Port_2, this, false, name: "Output"));
        }
        public override string Name => "Square Root";
        public override string Category => NodeCategoryType.Math;
        public override string Icon => "icon-calculator";
        public override void CalculateValue()
        {
            var input = Ports[0];
            if (!input.Links.Any())
            {
                SetValue(null!, false);
            }
            else
            {
                var input_val = GetInputValue(input, input.Links.First());

                if (input == null)
                {
                    SetValue(null!, false);
                }
                else
                {
                    Value = System.Math.Sqrt(input_val);
                }
            }
        }

    }
}
