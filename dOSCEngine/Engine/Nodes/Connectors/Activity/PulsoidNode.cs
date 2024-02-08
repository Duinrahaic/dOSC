using Blazor.Diagrams.Core.Geometry;
using dOSCEngine.Services.Connectors.OSC;
using dOSCEngine.Engine.Ports;
using dOSCEngine.Services.Connectors.Activity.Pulsoid;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using dOSCEngine.Services;
using System.Collections.Concurrent;

namespace dOSCEngine.Engine.Nodes.Connectors.Activity
{
    public class PulsoidNode : BaseNode
    {
        public PulsoidNode(Guid? guid = null, ConcurrentDictionary<EntityPropertyEnum, dynamic>? properties = null, Point? position = null, ServiceBundle? service = null) : base(guid, position, properties)
        {
            AddPort(new NumericPort(PortGuids.Port_1, this, false, "Heart Rate"));
            _service = service.Pulsoid;
            if (_service != null)
            {
                _service.OnPulsoidMessageReceived += _service_OnPulsoidMessageReceived;
            }
            Value = 0;
        }
        public override string Name => "Pulsoid";
        public override string Category => NodeCategoryType.Connector;
        public override string Icon => "icon-heart-pulse";
        private readonly PulsoidService? _service = null;
        private void _service_OnPulsoidMessageReceived(PulsoidReading e)
        {
            Value = e.Data.HeartRate;
            CalculateValue();
        }

        public override void OnDispose()
        {
            if (_service != null)
                _service.OnPulsoidMessageReceived -= _service_OnPulsoidMessageReceived;
        }
    }
}
