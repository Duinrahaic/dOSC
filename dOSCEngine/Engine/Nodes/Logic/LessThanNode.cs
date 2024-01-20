using Blazor.Diagrams.Core.Geometry;
using dOSCEngine.Engine.Ports;
using Newtonsoft.Json;
using System.Collections.Concurrent;

namespace dOSCEngine.Engine.Nodes.Logic
{
    public class LessThanNode : BaseNode
    {
        public LessThanNode(Guid? guid = null, ConcurrentDictionary<EntityPropertyEnum, dynamic>? properties = null, Point? position = null) : base(guid, position, properties)
        {
            AddPort(new NumericPort(PortGuids.Port_1, this, true, name: "Value A"));
            AddPort(new NumericPort(PortGuids.Port_2, this, true, name: "Value B"));
            AddPort(new LogicPort(PortGuids.Port_3, this, false, name: "Output"));
        }
        
        public override string Name => "Less Than";
        public override string Category => NodeCategoryType.Logic;
        public override string TextIcon => "<";

        public override void CalculateValue()
        {
            var inA = Ports[0];
            var inB = Ports[1];
            if (inA.Links.Any() && inB.Links.Any())
            {
                var l1 = inA.Links.First();
                var l2 = inB.Links.First();
                var ValA = GetInputValue(inA, l1);
                var ValB = GetInputValue(inB, l2);

                if (ValA != null && ValB != null)
                {
                    Value = Convert.ToBoolean(ValA) < Convert.ToBoolean(ValB);
                }
            }
            else
            {
                Value = false;
            }
        }

    }
}
