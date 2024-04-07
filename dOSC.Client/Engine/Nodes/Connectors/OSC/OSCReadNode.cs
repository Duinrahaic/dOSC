using System.Collections.Concurrent;
using Blazor.Diagrams.Core.Geometry;
using dOSC.Client.Engine.Ports;
using dOSC.Client.Services.Connectors.Hub.OSC;
using dOSC.Shared.Models.Wiresheet;
using dOSC.Shared.Utilities;
using OSCService = dOSC.Client.Services.Connectors.Hub.OSC.OSCService;
using ServiceBundle = dOSC.Client.Services.ServiceBundle;

namespace dOSC.Client.Engine.Nodes.Connectors.OSC;

public class OSCReadNode : BaseNode
{
    private readonly OSCService? _service;
    private bool _isAvatarParameter;
    private bool _writeAsFloat;
    public string OSCAddress;

    public OSCReadNode(Guid? guid = null, ConcurrentDictionary<EntityPropertyEnum, dynamic>? properties = null,
        Point? position = null, ServiceBundle? service = null) : base(guid, position, properties)
    {
        AddPort(new MultiPort(PortGuids.Port_1, this, false, limitLink: false, name: "Read Value",
            allowedTypes: new List<PortType> { PortType.Numeric, PortType.Logic, PortType.Multi }));
        _service = service?.OSC;
        Properties.TryInitializeProperty(EntityPropertyEnum.IsAvatarParameter, true);
        Properties.TryInitializeProperty(EntityPropertyEnum.OSCAddress, string.Empty);
        Properties.TryInitializeProperty(EntityPropertyEnum.WriteAsFloat, true);

        OSCAddress = Properties.GetProperty<string>(EntityPropertyEnum.OSCAddress);
        _isAvatarParameter = Properties.GetProperty<bool>(EntityPropertyEnum.IsAvatarParameter);
        _writeAsFloat = Properties.GetProperty<bool>(EntityPropertyEnum.WriteAsFloat);

        if (_service != null)
            _service.OnOSCMessageRecieved += OnOSCMessageReceived;
        VisualIndicator = OSCAddress ?? "No Parameter Set";
        VisualIndicator = string.IsNullOrEmpty(VisualIndicator) ? "No Parameter Set" : VisualIndicator;
    }

    public override string Name => "OSC Read Node";
    public override string Category => NodeCategoryType.Connector;
    public override string Icon => "icon-hard-drive-download";

    private string GetFullAddress()
    {
        return string.Join('/', $"{(_isAvatarParameter ? "/avatar/parameters" : string.Empty)}", OSCAddress);
    }

    private void OnOSCMessageReceived(OSCSubscriptionEvent e)
    {
        if (!string.IsNullOrEmpty(OSCAddress))
            if (e.Address.Equals(GetFullAddress(), StringComparison.OrdinalIgnoreCase))
                Value = GetNumeric((dynamic?)e.Arguments.FirstOrDefault(), _writeAsFloat);
    }

    public override void PropertyNotifyEvent(EntityPropertyEnum property, dynamic? value)
    {
        if (property == EntityPropertyEnum.OSCAddress)
        {
            OSCAddress = value;
            VisualIndicator = OSCAddress ?? "No Parameter Set";
            VisualIndicator = string.IsNullOrEmpty(VisualIndicator) ? "No Parameter Set" : VisualIndicator;
        }
        else if (property == EntityPropertyEnum.IsAvatarParameter)
        {
            _isAvatarParameter = value;
        }
        else if (property == EntityPropertyEnum.WriteAsFloat)
        {
            _writeAsFloat = value;
        }
    }


    private double? GetNumeric(dynamic? Value, bool WriteAsFloat = false)
    {
        dynamic? value = null;

        try
        {
            if (WriteAsFloat)
                value = Convert.ToDouble(Value);
            else
                value = Convert.ToDouble(Value);
            SetErrorState(false);
        }
        catch
        {
            SetErrorState(true);
        }

        return value;
    }

    private bool? GetLogic(dynamic? Value)
    {
        dynamic? value = null;
        try
        {
            if (Classifier.IsBooleanType(Value)) value = Math.Clamp(Convert.ToBoolean(Value), 0, 1);
            SetErrorState(false);
        }
        catch
        {
            SetErrorState(true);
        }

        return value;
    }

    public override void OnDispose()
    {
        if (_service != null)
            _service.OnOSCMessageRecieved -= OnOSCMessageReceived;
        UnsubscribeToAllPortTypeChanged();
        base.OnDispose();
    }
}