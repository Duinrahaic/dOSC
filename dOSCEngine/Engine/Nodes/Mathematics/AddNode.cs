using Blazor.Diagrams.Core.Anchors;
using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Models.Base;
using dOSCEngine.Engine.Ports;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Concurrent;

namespace dOSCEngine.Engine.Nodes.Mathematics
{

    public class AddNode : BaseNode
    {
        public AddNode(Guid? guid = null, ConcurrentDictionary<EntityPropertyEnum, dynamic>? properties = null, Point? position = null) : base(guid, position, properties)
        {
            AddPort(new NumericPort(PortGuids.Port_1, this, true, limitLink: false , name: "Value")); // Defines an Value Port A with a GUID of Port 1
			AddPort(new NumericPort(PortGuids.PortGuidGenerator(1000), this, false, name: "Output")); // Defines an Output Port with a GUID of Port 3
        }
        public override string Name => Links.Count <= 1 ? "Add" : "Sum";
        public override string Category => NodeCategoryType.Math;
        public override string Icon => "icon-plus-circle";
        public override void CalculateValue()
        {
            if (!Ports.First().Links.Any())
                return;

            dynamic result;
            if (Links.Count <= 1)
            {
                dynamic value = GetInputValue(Ports.First(), Ports.First().Links.First());
                if(value != null)
                {
                    result = value;
                }
                else
                {
                    SetValue(null!, false);
                    return;
                }
            }
            else
            {
                List<double> Values = new();
                foreach(var link in Ports.First().Links)
                {
                    dynamic value = GetInputValue(Ports.First(), link);
                    if (value != null)
                    {
                        Values.Add(value);
                    }

                    if (!Values.Any())
                    {
                        SetValue(null!, false);
                        return;
                    }
                }
                result = Values.Sum();
            }
            Value = result;
        }
    }

}
