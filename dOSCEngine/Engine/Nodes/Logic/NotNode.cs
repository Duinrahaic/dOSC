using Blazor.Diagrams.Core.Anchors;
using Blazor.Diagrams.Core.Models.Base;
using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Geometry;
using Newtonsoft.Json;
using dOSCEngine.Engine.Ports;
using System.Collections.Concurrent;

namespace dOSCEngine.Engine.Nodes.Logic
{
    public class NotNode : BaseNode
    {
        public NotNode(Guid? guid = null, ConcurrentDictionary<EntityPropertyEnum, dynamic>? properties = null, Point? position = null) : base(guid, position, properties)
        {
            AddPort(new LogicPort(PortGuids.Port_1, this, true, name: "Value A"));
            AddPort(new LogicPort(PortGuids.Port_2, this, false, name: "Output"));
        }
        public override string Name => "Not";
        public override string Category => NodeCategoryType.Logic;
        public override string TextIcon => "!A";
        public override void CalculateValue()
        {
            var inA = Ports[0];
            if (inA.Links.Any())
            {
                var l1 = inA.Links.First();
                var ValA = GetInputValue(inA, l1);

                if (ValA != null)
                {
                    Value = !Convert.ToBoolean(ValA);
                }
            }
            else
            {
                Value = false;
            }
        }


    }
}
