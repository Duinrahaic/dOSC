using Blazor.Diagrams.Core.Geometry;
using dOSCEngine.Services.Connectors.OSC;
using dOSCEngine.Engine.Ports;
using Newtonsoft.Json;
using dOSCEngine.Services;

namespace dOSCEngine.Engine.Nodes.Connectors.OSC
{
    public class OSCBooleanReadNode : BaseNode, IDisposable
    {
        public OSCBooleanReadNode(ServiceBundle? service = null, Point? position = null) : base(position ?? new Point(0, 0))
        {
            AddPort(new LogicPort(PortGuids.Port_1, this, false));
            _service = service?.OSC;
            SelectedOption = string.Empty;
            if (_service != null)
            {
                _service.OnOSCMessageRecieved += OnMessageReceived;
            }
        }
        public OSCBooleanReadNode(string? SelectedOption, ServiceBundle? service = null, Point? position = null) : base(position ?? new Point(0, 0))
        {
            AddPort(new LogicPort(PortGuids.Port_1, this, false));
            _service = service?.OSC;
            this.SelectedOption = string.IsNullOrEmpty(SelectedOption) ? string.Empty : SelectedOption;
            if (_service != null)
            {
                _service.OnOSCMessageRecieved += OnMessageReceived;
            }
        }
        public OSCBooleanReadNode(Guid guid, string? SelectedOption, ServiceBundle? service = null, Point? position = null) : base(guid, position ?? new Point(0, 0))
        {
            AddPort(new LogicPort(PortGuids.Port_1, this, false));
            _service = service?.OSC;
            this.SelectedOption = string.IsNullOrEmpty(SelectedOption) ? string.Empty : SelectedOption;
            if (_service != null)
            {
                _service.OnOSCMessageRecieved += OnMessageReceived;
            }
        }

        [JsonProperty]
        public override string NodeClass => GetType().Name.ToString();
        private readonly OSCService? _service = null;
        [JsonProperty]
        public override string Option => SelectedOption;
        public string SelectedOption { get; set; } = string.Empty;
        public override string BlockTypeClass => "connectorblock";

        private void OnMessageReceived(OSCSubscriptionEvent e)
        {
            if (SelectedOption != null)
            {
                if (e.Address.ToLower() == SelectedOption.ToLower())
                {
                    var val = Convert.ToBoolean(e.Arguments.First());
                    Value = val;
                }
            }
        }

        public void Dispose()
        {
            _service.OnOSCMessageRecieved -= OnMessageReceived;
        }
    }
}
