using System.Collections.Concurrent;
using Blazor.Diagrams.Core.Geometry;
using dOSC.Client.Engine.Ports;
using dOSC.Shared.Models.Wiresheet;

namespace dOSC.Client.Engine.Nodes.Utility
{
    public class InvertNode: BaseNode
    {
        public InvertNode(Guid? guid = null, ConcurrentDictionary<EntityPropertyEnum, dynamic>? properties = null, Point? position = null) : base(guid, position, properties)
        {
            AddPort(new MultiPort(PortGuids.Port_1, this, true, allowedTypes: new() { PortType.Numeric, PortType.Logic, PortType.Multi }, name: "Value"));
            AddPort(new MultiPort(PortGuids.Port_2, this, false, allowedTypes: new() { PortType.Numeric, PortType.Logic, PortType.Multi }, name: "Output"));
            SubscribeToAllPortTypeChanges();
        }
        public override string Name => "Invert";
        public override string Category => NodeCategoryType.Utilities;
        public override string Icon => "icon-circle-slash-2";
        public override void CalculateValue()
        {
            var input = Ports[0];
            if (!input.Links.Any())
            {
                SetValue(null!, false);
            }
            else
            {
                var LV = GetInputValue(input, input.Links.First());
                PortType? PT = GetCurrentMultiPortType();
                if (PT.HasValue)
                {
                    if (LV != null)
                    {
                        if (PT == PortType.Numeric)
                        {
                            Value = LV * -1;
                        }
                        else if (PT == PortType.Logic)
                        {
                            Value = !LV;
                        }
                        else
                        {
                            SetValue(null!, false);
                        }
                    }
                    else
                    {
                        SetValue(null!, false);
                    }
                }
                else
                {
                    SetValue(null!, false);
                }

            }


        }

        public override void OnDispose()
        {
            UnsubscribeToAllPortTypeChanged();
            base.OnDispose();
        }
    }

}
