using Blazor.Diagrams.Core.Geometry;
using dOSCEngine.Services.Connectors.OSC;
using dOSCEngine.Engine.Nodes;
using dOSCEngine.Engine.Ports;
using dOSCEngine.Services.Connectors.Activity.Pulsoid;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace dOSCEngine.Engine.Nodes.Connector.Activity
{
    public class PulsoidNode : BaseNode, IDisposable
    {
        public PulsoidNode(PulsoidService? service = null, Point? position = null) : base(position ?? new Point(0, 0))
        {
            AddPort(new NumericPort(PortGuids.Port_1, this, false));
            _service = service;
            if (_service != null)
            {
                _service.OnPulsoidMessageReceived += _service_OnPulsoidMessageReceived;
            }
            Value = 0;
        }
        public PulsoidNode(Guid guid, PulsoidService? service = null, Point? position = null) : base(guid, position ?? new Point(0, 0))
        {
            AddPort(new NumericPort(PortGuids.Port_1, this, false));
            _service = service;
            if (_service != null)
            {
                _service.OnPulsoidMessageReceived += _service_OnPulsoidMessageReceived;
            }
            Value = 0;
        }
        [JsonProperty]
        public override string NodeClass => GetType().Name.ToString();
        private readonly PulsoidService? _service = null;
        public override string BlockTypeClass => "connectorblock";
        private void _service_OnPulsoidMessageReceived(PulsoidReading e)
        {
            Value = e.Data.HeartRate;
            CalculateValue();
        }

        public void Dispose()
        {
            if (_service != null)
                _service.OnPulsoidMessageReceived -= _service_OnPulsoidMessageReceived;
        }
    }
}
