using Blazor.Diagrams.Core.Geometry;
using dOSC.Engine.Ports;
using dOSC.Services.Connectors.OSC;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace dOSC.Engine.Nodes.Connector.OSC
{
    public class OSCIntReadNode : BaseNode
    {
        public OSCIntReadNode(OSCService? service = null, Point? position = null) : base(position ?? new Point(0, 0))
        {
            AddPort(new NumericPort(PortGuids.Port_1, this, false));
            _service = service;
            this.SelectedOption = string.Empty;
            if (_service != null)
            {
                _service.OnOSCMessageRecieved += OnMessageRecieved;
            }
        }


        public OSCIntReadNode(string? SelectedOption, OSCService? service = null, Point? position = null) : base(position ?? new Point(0, 0))
        {
            AddPort(new NumericPort(PortGuids.Port_1, this, false));
            _service = service;
            this.SelectedOption = string.IsNullOrEmpty(SelectedOption) ? string.Empty : SelectedOption;
        }
        public OSCIntReadNode(Guid guid, string? SelectedOption, OSCService? service = null, Point? position = null) : base(guid, position ?? new Point(0, 0))
        {
            AddPort(new NumericPort(PortGuids.Port_1, this, false));
            _service = service;
            this.SelectedOption = string.IsNullOrEmpty(SelectedOption) ? string.Empty : SelectedOption;
        }

        [JsonProperty]
        public override string NodeClass => this.GetType().Name.ToString();
        private readonly OSCService? _service = null;
        [JsonProperty]
        public override string Option => SelectedOption;
        public string SelectedOption { get; set; } = string.Empty;
        public override string BlockTypeClass => "connectorblock";

        private void OnMessageRecieved(OSCSubscriptionEvent e)
        {
            if(SelectedOption != null)
            {
                if (e.Address.ToLower() == SelectedOption.ToLower())
                { 
                    Value = e.Arguments.First();
                }
            }
        }

    }
}
