using System.Collections.Concurrent;
using Blazor.Diagrams.Core.Geometry;
using dOSC.Client.Engine.Ports;
using dOSC.Client.Services;
using dOSC.Client.Services.Connectors.Hub.Activity.Pulsoid;
using dOSC.Shared.Models.Wiresheet;

namespace dOSC.Client.Engine.Nodes.Connectors.Activity;

public class PulsoidNode : BaseNode
{
    private readonly PulsoidService? _service;

    public PulsoidNode(Guid? guid = null, ConcurrentDictionary<EntityPropertyEnum, dynamic>? properties = null,
        Point? position = null, ServiceBundle? service = null) : base(guid, position, properties)
    {
        AddPort(new NumericPort(PortGuids.Port_1, this, false, "Heart Rate"));
        _service = service.Pulsoid;
        if (_service != null) _service.OnPulsoidMessageReceived += _service_OnPulsoidMessageReceived;
        Value = 0;
    }

    public override string Name => "Pulsoid";
    public override string Category => NodeCategoryType.Connector;
    public override string Icon => "icon-heart-pulse";

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