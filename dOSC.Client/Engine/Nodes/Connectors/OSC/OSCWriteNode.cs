using System.Collections.Concurrent;
using Blazor.Diagrams.Core.Geometry;
using dOSC.Client.Engine.Ports;
using dOSC.Shared.Models.Wiresheet;
using OSCService = dOSC.Client.Services.Connectors.Hub.OSC.OSCService;
using ServiceBundle = dOSC.Client.Services.ServiceBundle;

namespace dOSC.Client.Engine.Nodes.Connectors.OSC;

public class OSCWriteNode : BaseNode
{
    private readonly OSCService? _service;

    private readonly object _updateLock = new();
    private bool _isAvatarParameter;
    private bool _sendChatMessageImmediately;
    private bool _sendChatMessageWithSound;
    private bool _writeAsFloat;
    public string OSCAddress;

    public OSCWriteNode(Guid? guid = null, ConcurrentDictionary<EntityPropertyEnum, dynamic>? properties = null,
        Point? position = null, ServiceBundle? service = null) : base(guid, position, properties)
    {
        AddPort(new MultiPort(PortGuids.Port_1, this, true, "Set Value",
            allowedTypes: new List<PortType> { PortType.Numeric, PortType.String, PortType.Logic, PortType.Multi }));
        _service = service?.OSC;
        Properties.TryInitializeProperty(EntityPropertyEnum.IsAvatarParameter, true);
        Properties.TryInitializeProperty(EntityPropertyEnum.OSCAddress, string.Empty);
        Properties.TryInitializeProperty(EntityPropertyEnum.WriteAsFloat, true);
        Properties.TryInitializeProperty(EntityPropertyEnum.WriteAsFloat, true);
        Properties.TryInitializeProperty(EntityPropertyEnum.SendChatMessageImmediately, true);
        Properties.TryInitializeProperty(EntityPropertyEnum.SendChatMessageWithSound, true);

        OSCAddress = Properties.GetProperty<string>(EntityPropertyEnum.OSCAddress);
        _isAvatarParameter = Properties.GetProperty<bool>(EntityPropertyEnum.IsAvatarParameter);
        _writeAsFloat = Properties.GetProperty<bool>(EntityPropertyEnum.WriteAsFloat);
        _sendChatMessageImmediately = Properties.GetProperty<bool>(EntityPropertyEnum.SendChatMessageImmediately);
        _sendChatMessageWithSound = Properties.GetProperty<bool>(EntityPropertyEnum.SendChatMessageWithSound);
        SubscribeToAllPortTypeChanges();
        VisualIndicator = OSCAddress ?? "No Parameter Set";
        VisualIndicator = string.IsNullOrEmpty(VisualIndicator) ? "No Parameter Set" : VisualIndicator;
    }

    public override string Name => "OSC Write Node";
    public override string Category => NodeCategoryType.Connector;
    public override string Icon => "icon-hard-drive-upload";

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
        else if (property == EntityPropertyEnum.SendChatMessageImmediately)
        {
            _sendChatMessageImmediately = value;
        }
        else if (property == EntityPropertyEnum.SendChatMessageWithSound)
        {
            _sendChatMessageWithSound = value;
        }
    }

    public override void CalculateValue()
    {
        lock (_updateLock)
        {
            if (_service != null)
                if (!string.IsNullOrEmpty(OSCAddress))
                {
                    var input = Ports.First() as BasePort;
                    if (input?.HasValidLinks() ?? false)
                    {
                        var linkValue = input.GetInputValue();
                        if (linkValue != null)
                        {
                            var portType = GetCurrentMultiPortType();
                            if (portType.HasValue)
                            {
                                if (linkValue == null) return;
                                switch (portType)
                                {
                                    case PortType.Numeric:
                                        SendNumeric(GetFullAddress(), linkValue, _writeAsFloat);
                                        break;
                                    case PortType.String:
                                        SendChatMessage(linkValue, _sendChatMessageWithSound,
                                            _sendChatMessageImmediately);
                                        break;
                                    case PortType.Logic:
                                        SendBool(GetFullAddress(), linkValue);
                                        break;
                                }
                            }
                        }
                    }
                }
        }
    }

    private string GetFullAddress()
    {
        return string.Join('/', $"{(_isAvatarParameter ? "/avatar/parameters" : string.Empty)}", OSCAddress);
    }

    private void SendNumeric(string Address, dynamic Value, bool WriteAsFloat = false)
    {
        if (WriteAsFloat)
            Value = Math.Clamp(Convert.ToInt32(Value), -1, 1);
        else
            Value = Math.Clamp(Convert.ToInt32(Value), 0, 255);
        if (_service != null) _service.SendMessage(Address, Value);
    }

    private void SendBool(string Address, dynamic Value)
    {
        Value = Math.Clamp(Convert.ToInt32(Value), 0, 1);
        if (_service != null) _service.SendMessage(Address, Value);
    }

    private void SendChatMessage(dynamic Value, bool UseSound = false, bool QuickMessage = false)
    {
        if (_service != null) _service.SendChatMessage(Value, QuickMessage, UseSound);
    }

    public override void OnDispose()
    {
        UnsubscribeToAllPortTypeChanged();
        base.OnDispose();
    }
}