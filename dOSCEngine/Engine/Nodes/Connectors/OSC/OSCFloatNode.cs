using Blazor.Diagrams.Core.Geometry;
using dOSCEngine.Services.Connectors.OSC;
using dOSCEngine.Engine.Ports;
using Newtonsoft.Json;
using dOSCEngine.Services;

namespace dOSCEngine.Engine.Nodes.Connectors.OSC
{
    public class OSCFloatNode : BaseNode
    {
        public OSCFloatNode(ServiceBundle? service = null, Point? position = null) : base(position ?? new Point(0, 0))
        {
            AddPort(new NumericPort(PortGuids.Port_1, this, true));
            _service = service?.OSC;
            SelectedOption = string.Empty;
        }
        public OSCFloatNode(string? SelectedOption, ServiceBundle? service = null, Point? position = null) : base(position ?? new Point(0, 0))
        {
            AddPort(new NumericPort(PortGuids.Port_1, this, true));
            _service = service?.OSC;
            this.SelectedOption = string.IsNullOrEmpty(SelectedOption) ? string.Empty : SelectedOption;
        }
        public OSCFloatNode(Guid guid, string? SelectedOption, ServiceBundle? service = null, Point? position = null) : base(guid, position ?? new Point(0, 0))
        {
            AddPort(new NumericPort(PortGuids.Port_1, this, true));
            _service = service?.OSC;
            this.SelectedOption = string.IsNullOrEmpty(SelectedOption) ? string.Empty : SelectedOption;
        }

        [JsonProperty]
        public override string NodeClass => GetType().Name.ToString();
        private readonly OSCService? _service = null;
        [JsonProperty]
        public override string Option => SelectedOption;
        public string SelectedOption { get; set; } = string.Empty;
        public override string BlockTypeClass => "connectorblock";

        public override void CalculateValue()
        {
            if (_service != null)
            {
                var input = Ports.First();
                if (input.Links.Any())
                {
                    var i = GetInputValue(input, input.Links.First());
                    var v = Convert.ToInt32(i);
                    v = System.Math.Clamp(v, -1, 1);
                    _service.SendMessage(SelectedOption, v);
                }
            }
        }
    }
}
