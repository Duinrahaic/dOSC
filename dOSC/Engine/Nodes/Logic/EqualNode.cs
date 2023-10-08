using Blazor.Diagrams.Core.Geometry;
using dOSC.Engine.Ports;
using dOSC.Services.Connectors.OSC;
using Microsoft.AspNetCore.Components;
using static dOSC.Services.Connectors.OSC.OSCService;

namespace dOSC.Engine.Nodes.Logic
{
    public class EqualNode : BaseNode
    {
        public EqualNode(Point? position = null) : base(position ?? new Point(0, 0))
        {
            _PortA = new MultiPort(this, true);
            _PortB = new MultiPort(this, true);
            AddPort(_PortA);
            AddPort(_PortB);
            AddPort(new LogicPort(this, false));
        }

        private MultiPort _PortA;
        private MultiPort _PortB;
        private string _PortAType = "multi";
        private string _PortBType = "multi";
        public string PortAType => _PortAType;
        public string PortBType => _PortBType;
        public override string BlockTypeClass => "logicblock";


        public override void Refresh()
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
                if(valA.GetType() == valB.GetType())
                {
                Value = valA == valB;
                    this.ErrorMessage = string.Empty;
                    this.Error = false;
            }
            else
            {
                    this.ErrorMessage = "Cannot compare two different data types!";
                    this.Error = true;
                }
            }
            else
            {
                this.Error = false;
                this.ErrorMessage = string.Empty;
                Value = false;
            }
            base.Refresh();
        }


        private void ConfigurePortAType()
        {
            var inA = Ports[0];
            if (inA.Links.Any())
            {
                var l1 = inA.Links.First();
                var valA = GetInputValue(inA, l1);
                switch (Type.GetTypeCode(valA.GetType()))
                {
                    case TypeCode.Double:
                        _PortAType = "numeric";
                        break;
                    case TypeCode.Int16:
                        _PortAType = "numeric";
                        break;
                    case TypeCode.Int32:
                        _PortAType = "numeric";
                        break;
                    case TypeCode.String:
                        _PortAType = "string";
                        break;
                    case TypeCode.Boolean:
                        _PortAType = "logic";
                        break;
                    default:
                        _PortAType = "multi";
                        break;
                }
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
                var val = GetInputValue(inB, l1);
                switch (Type.GetTypeCode(val.GetType()))
                {
                    case TypeCode.Double:
                        _PortBType = "numeric";
                        break;
                    case TypeCode.Int16:
                        _PortBType = "numeric";
                        break;
                    case TypeCode.Int32:
                        _PortBType = "numeric";
                        break;
                    case TypeCode.String:
                        _PortBType = "string";
                        break;
                    case TypeCode.Boolean:
                        _PortBType = "logic";
                        break;
                    default:
                        _PortBType = "multi";
                        break;
                }
            }
            else
            {
                _PortBType = "multi";
            }
            inB.Refresh();
        }
    }
}
