using Blazor.Diagrams.Core.Geometry;
using dOSCEngine.Services.Connectors.OSC;
using dOSCEngine.Engine.Ports;
using Newtonsoft.Json;

namespace dOSCEngine.Engine.Nodes.Connector.OSC
{
    public class OSCBooleanReadNode : BaseNode
    {
        public OSCBooleanReadNode(OSCService? service = null, Point? position = null) : base(position ?? new Point(0, 0))
        {
            AddPort(new LogicPort(PortGuids.Port_1, this, false));
            _service = service;
            SelectedOption = string.Empty;
            if (_service != null)
            {
                _service.OnOSCMessageRecieved += OnMessageRecieved;
            }
        }
        public OSCBooleanReadNode(string? SelectedOption, OSCService? service = null, Point? position = null) : base(position ?? new Point(0, 0))
        {
            AddPort(new LogicPort(PortGuids.Port_1, this, false));
            _service = service;
            this.SelectedOption = string.IsNullOrEmpty(SelectedOption) ? string.Empty : SelectedOption;
            if (_service != null)
            {
                _service.OnOSCMessageRecieved += OnMessageRecieved;
            }
        }
        public OSCBooleanReadNode(Guid guid, string? SelectedOption, OSCService? service = null, Point? position = null) : base(guid, position ?? new Point(0, 0))
        {
            AddPort(new LogicPort(PortGuids.Port_1, this, false));
            _service = service;
            this.SelectedOption = string.IsNullOrEmpty(SelectedOption) ? string.Empty : SelectedOption;
            if (_service != null)
            {
                _service.OnOSCMessageRecieved += OnMessageRecieved;
            }
        }

        [JsonProperty]
        public override string NodeClass => GetType().Name.ToString();
        private readonly OSCService? _service = null;
        [JsonProperty]
        public override string Option => SelectedOption;
        public string SelectedOption { get; set; } = string.Empty;
        public override string BlockTypeClass => "connectorblock";

        private void OnMessageRecieved(OSCSubscriptionEvent e)
        {
            if (SelectedOption != null)
            {
                if (e.Address.ToLower() == SelectedOption.ToLower())
                {
                    try
                    {
                        var val = Convert.ToInt32(e.Arguments.First());
                        Value = System.Math.Clamp(val, 0, 1);
                    }
                    catch
                    {

                    }

                }
            }
        }
    }
}
