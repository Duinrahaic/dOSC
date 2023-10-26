using Blazor.Diagrams.Core.Geometry;
using dOSC.Engine.Ports;
using dOSC.Services.Connectors.Activity.Pulsoid;
using dOSC.Services.Connectors.OSC;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace dOSC.Engine.Nodes.Connector.Activity
{
    public class PulsoidNode: BaseNode, IDisposable
    {
        public PulsoidNode(PulsoidService? service = null, Point? position = null) : base(position ?? new Point(0, 0))
        {
            AddPort(new NumericPort(PortGuids.Port_1, this, false));
             _service = service;
            if(_service != null)
            {
                _service.OnPulsoidMessageRecieved += _service_OnPulsoidMessageRecieved;
            }
         }
        public PulsoidNode(Guid guid, PulsoidService? service = null, Point? position = null) : base(guid, position ?? new Point(0, 0))
        {
            AddPort(new NumericPort(PortGuids.Port_1, this, false));
            _service = service;
            if (_service != null)
            {
                _service.OnPulsoidMessageRecieved += _service_OnPulsoidMessageRecieved;
            }
        }
        [JsonProperty]
        public override string NodeClass => this.GetType().Name.ToString();
        private readonly PulsoidService? _service = null;
        public override string BlockTypeClass => "connectorblock";
        private void _service_OnPulsoidMessageRecieved(PulsoidReading e)
        {
            Value = e.Data.HeartRate;
        }

        public void Dispose()
        {
            if( _service != null) 
                _service.OnPulsoidMessageRecieved -= _service_OnPulsoidMessageRecieved;
        }
    }
}
