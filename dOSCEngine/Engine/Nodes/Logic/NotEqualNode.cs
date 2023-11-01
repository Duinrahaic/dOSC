using Blazor.Diagrams.Core.Geometry;
using dOSCEngine.Engine.Nodes;
using dOSCEngine.Engine.Ports;
using Newtonsoft.Json;

namespace dOSCEngine.Engine.Nodes.Logic
{
    public class NotEqualNode : BaseNode
    {
        public NotEqualNode(Point? position = null) : base(position ?? new Point(0, 0))
        {
            _PortA = new MultiPort(PortGuids.Port_1, this, true);
            _PortB = new MultiPort(PortGuids.Port_2, this, true);
            AddPort(_PortA);
            AddPort(_PortB);
            AddPort(new LogicPort(PortGuids.Port_3, this, false));
        }
        public NotEqualNode(Guid guid, Point? position = null) : base(guid, position ?? new Point(0, 0))
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
                if (valA.GetType() == valB.GetType())
                {
                    Value = valA != valB;
                    ErrorMessage = string.Empty;
                    Error = false;
                }
                else
                {
                    ErrorMessage = "Cannot compare two different data types!";
                    Error = true;
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