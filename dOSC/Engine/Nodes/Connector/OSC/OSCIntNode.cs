using Blazor.Diagrams.Core.Geometry;
using dOSC.Engine.Ports;
using dOSC.Services.Connectors.OSC;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace dOSC.Engine.Nodes.Connector.OSC
{
    public class OSCIntNode : BaseNode
    {
        public OSCIntNode(OSCService? service = null, Point? position = null) : base(position ?? new Point(0, 0))
        {
            AddPort(new NumericPort(PortGuids.Port_1, this, true));
            _service = service;
            this.SelectedOption = string.Empty;
        }
        public OSCIntNode(string? SelectedOption, OSCService? service = null, Point? position = null) : base(position ?? new Point(0, 0))
        {
            AddPort(new NumericPort(PortGuids.Port_1, this, true));
            _service = service;
            this.SelectedOption = string.IsNullOrEmpty(SelectedOption) ? string.Empty : SelectedOption;
        }
        public OSCIntNode(Guid guid, string? SelectedOption, OSCService? service = null, Point? position = null) : base(guid, position ?? new Point(0, 0))
        {
            AddPort(new NumericPort(PortGuids.Port_1, this, true));
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

        public override void Refresh()
        {
            if (_service != null)
            {
                var input = Ports.First();
                if (input.Links.Any())
                {
                    var i = GetInputValue(input, input.Links.First());
                    var v = Convert.ToInt32(i);
                    v = System.Math.Clamp(v, 0, 255);
                    _service.SendMessage(SelectedOption, v);
                }
            }
            base.Refresh();
        }
    }
}
