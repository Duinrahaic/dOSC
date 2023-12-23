using Blazor.Diagrams.Core.Geometry;
using dOSCEngine.Services.Connectors.OSC;
using dOSCEngine.Engine.Ports;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using static dOSCEngine.Services.Connectors.OSC.OSCService;
using Blazor.Diagrams.Core.Anchors;

namespace dOSCEngine.Engine.Nodes.Logic
{
    public class EqualNode : BaseNode
    {
        public EqualNode(Point? position = null) : base(position ?? new Point(0, 0))
        {
            _PortA = new MultiPort(PortGuids.Port_1, this, true);
            _PortB = new MultiPort(PortGuids.Port_2, this, true);
            AddPort(_PortA);
            AddPort(_PortB);
            AddPort(new LogicPort(PortGuids.Port_3, this, false));
        }
        public EqualNode(Guid guid, Point? position = null) : base(guid, position ?? new Point(0, 0))
        {
            _PortA = new MultiPort(PortGuids.Port_1, this, true);
            _PortB = new MultiPort(PortGuids.Port_2, this, true);
            AddPort(_PortA);
            AddPort(_PortB);
            AddPort(new LogicPort(PortGuids.Port_3, this, false));
        }
        [JsonProperty]
        public override string NodeClass => GetType().Name.ToString();
        private MultiPort _PortA;
        private MultiPort _PortB;
        private string _PortAType = "multi";
        private string _PortBType = "multi";
        public string PortAType => _PortAType;
        public string PortBType => _PortBType;
        public override string BlockTypeClass => "logicblock";





        public override void CalculateValue()
        {
            var inA = Ports[0];
            var inB = Ports[1];

            ConfigurePortAType();
            ConfigurePortBType();
            if (inA.Links.Any() && inB.Links.Any())
            {
                var l1 = inA.Links.First();
                var l2 = inB.Links.First();

                var valA = GetInputValue(inA, l1);
                var valB = GetInputValue(inB, l2);
                if (valA != null && valB != null)
                {
                    if (valA.GetType() == valB.GetType())
                    {
                        Value = valA == valB;
                        ErrorMessage = string.Empty;
                        Error = false;
                    }
                    else
                    {
                        ErrorMessage = "Cannot compare two different data types!";
                        Error = true;
                    }
                }
            }
            else
            {
                Error = false;
                ErrorMessage = string.Empty;
                Value = false;
            }
        }



        private void ConfigurePortAType()
        {
            var inA = Ports[0];
            if (inA.Links.Any())
            {
                var l1 = inA.Links.First();
                var sp = (l1.Source as SinglePortAnchor)!;
                var tp = (l1.Source as SinglePortAnchor)!;
                var p = sp.Port.Parent.Id == Id ? tp : sp;
                var port = p.Port as BasePort;
                if (port is MultiPort)
                    _PortAType = "multi";
                if (port is LogicPort)
                    _PortAType = "logic";
                if (port is NumericPort)
                    _PortAType = "numeric";
                if (port is StringPort)
                    _PortAType = "string";
            }
            else
            {
                _PortAType = "multi";
            }
            inA.Refresh();
        }
        private void ConfigurePortBType()
        {
            var inB = Ports[1];
            if (inB.Links.Any())
            {
                var l1 = inB.Links.First();
                var sp = (l1.Source as SinglePortAnchor)!;
                var tp = (l1.Target as SinglePortAnchor)!;
                var p = sp.Port.Parent.Id == Id ? tp : sp;
                var port = p.Port as BasePort;
                if (port is MultiPort)
                    _PortBType = "multi";
                if (port is LogicPort)
                    _PortBType = "logic";
                if (port is NumericPort)
                    _PortBType = "numeric";
                if (port is StringPort)
                    _PortBType = "string";
            }
            else
            {
                _PortBType = "multi";
            }
            inB.Refresh();
        }
    }
}
