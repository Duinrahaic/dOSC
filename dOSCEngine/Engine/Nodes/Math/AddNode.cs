using Blazor.Diagrams.Core.Anchors;
using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Models.Base;
using dOSCEngine.Engine.Nodes;
using dOSCEngine.Engine.Ports;
using Newtonsoft.Json;

namespace dOSCEngine.Engine.Nodes.Math
{

    public class AddNode : BaseNode
    {
        public AddNode(Point? position = null) : base(position ?? new Point(0, 0)) // Constructor for a new node
		{
            AddPort(new NumericPort(PortGuids.Port_1, this, true)); // Defines an Input Port A with a GUID of Port 1
            AddPort(new NumericPort(PortGuids.Port_2, this, true)); // Defines an Input Port B with a GUID of Port 2
            AddPort(new NumericPort(PortGuids.Port_3, this, false)); // Defines an Output Port with a GUID of Port 3
        }
        public AddNode(Guid guid, Point? position = null) : base(guid, position ?? new Point(0, 0)) // Constructor for an existing node
        {
			AddPort(new NumericPort(PortGuids.Port_1, this, true)); // Defines an Input Port A with a GUID of Port 1
			AddPort(new NumericPort(PortGuids.Port_2, this, true)); // Defines an Input Port B with a GUID of Port 2
			AddPort(new NumericPort(PortGuids.Port_3, this, false)); // Defines an Output Port with a GUID of Port 3
		}
        [JsonProperty]
        public override string NodeClass => GetType().Name.ToString();
        public override string BlockTypeClass => "numericblock";


        public override void CalculateValue()
        {
            var i1 = Ports[0];
            var i2 = Ports[1];

            if (i1.Links.Any() && i2.Links.Any()) // If both have links associated with it
            {
                var l1 = i1.Links[0]; // Get first link associated with input A
                var l2 = i2.Links[0]; // Get first link associated with input B
                var v1 = GetInputValue(i1, l1); //Get the value for input A
                var v2 = GetInputValue(i2, l2); //Get the value for input B
                Value = v1 + v2; // Set Value to the sum of A and B

            }
            else if (i1.Links.Any()) 
            {
				var l1 = i1.Links[0]; // Get first link associated with input A
				var v1 = GetInputValue(i1, l1); //Get the value for input A

				Value = v1; // Set Value to the value of A

            }
            else if (i2.Links.Any())
            {
				var l2 = i2.Links[0]; // Get first link associated with input B
				var v2 = GetInputValue(i2, l2); //Get the value for input B

				Value = v2; // Set Value to the value of A
			}
			else
            {
                Value = 0; // If no links exist then set Value to 0
            }
        }
    }

}
