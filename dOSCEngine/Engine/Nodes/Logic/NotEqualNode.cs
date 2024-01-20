using Blazor.Diagrams.Core.Geometry;
using dOSCEngine.Engine.Ports;
using Newtonsoft.Json;
using System.Collections.Concurrent;

namespace dOSCEngine.Engine.Nodes.Logic
{
    public class NotEqualNode : BaseNode
    {
        public NotEqualNode(Guid? guid = null, ConcurrentDictionary<EntityPropertyEnum, dynamic>? properties = null, Point? position = null) : base(guid, position, properties)
        {
            AddPort(new MultiPort(PortGuids.Port_1, this, true, name: "Value A"));
            AddPort(new MultiPort(PortGuids.Port_2, this, true, name: "Value B"));
            AddPort(new LogicPort(PortGuids.Port_3, this, false, name: "Output"));
            SubscribeToAllPortTypeChanges();
        }

        public override string Name => "Not Equal";
        public override string Category => NodeCategoryType.Logic;
        public override string TextIcon => "!=";



        public override void CalculateValue()
        {
            var inA = Ports[0];
            var inB = Ports[1];

            if (inA.Links.Any() && inB.Links.Any())
            {
                var l1 = inA.Links.First();
                var l2 = inB.Links.First();

                var valA = GetInputValue(inA, l1);
                var valB = GetInputValue(inB, l2);
                if (valA != null && valB != null)
                {

                    if (GetCurrentMultiPortType() == PortType.Logic)
                    {
                        Value = !(Convert.ToBoolean(valA) && Convert.ToBoolean(valB));
                    }
                    else if (GetCurrentMultiPortType() == PortType.String)
                    {
                        string strA = Convert.ToString(valA) ?? string.Empty;
                        string strB = Convert.ToString(valB) ?? string.Empty;
                        Value = !strA.Equals(strB);
                    }
                    else if (GetCurrentMultiPortType() != PortType.Numeric)
                    {
                        Value = Convert.ToDouble(valA) == Convert.ToDouble(valB);
                    }
                    else
                    {
                        SetValue(null!, false);
                    }

                }
            }
            else
            {
                Value = false;
            }
        }

        public override void OnDispose()
        {
            UnsubscribeToAllPortTypeChanged();
            base.OnDispose();
        }
    }
}