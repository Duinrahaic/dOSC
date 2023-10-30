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
        public AddNode(Point? position = null) : base(position ?? new Point(0, 0))
        {
            AddPort(new NumericPort(PortGuids.Port_1, this, true));
            AddPort(new NumericPort(PortGuids.Port_2, this, true));
            AddPort(new NumericPort(PortGuids.Port_3, this, false));
        }
        public AddNode(Guid guid, Point? position = null) : base(guid, position ?? new Point(0, 0))
        {
            AddPort(new NumericPort(PortGuids.Port_1, this, true));
            AddPort(new NumericPort(PortGuids.Port_2, this, true));
            AddPort(new NumericPort(PortGuids.Port_3, this, false));
        }
        [JsonProperty]
        public override string NodeClass => GetType().Name.ToString();
        public override string BlockTypeClass => "numericblock";


        public override void Refresh()
        {
            var i1 = Ports[0];
            var i2 = Ports[1];



            if (i1.Links.Count > 0 && i2.Links.Count > 0)
            {
                var l1 = i1.Links[0];
                var l2 = i2.Links[0];
                var v1 = GetInputValue(i1, l1);
                var v2 = GetInputValue(i2, l2);
                Value = v1 + v2;

            }
            else if (i1.Links.Count > 0)
            {
                var l1 = i1.Links[0];
                var v1 = GetInputValue(i1, l1);

                Value = v1;

            }
            else if (i2.Links.Count > 0)
            {
                var l2 = i2.Links[0];
                var v2 = GetInputValue(i2, l2);

                Value = v2;
            }
            else
            {
                Value = 0;
            }



            base.Refresh();
        }




    }

}
