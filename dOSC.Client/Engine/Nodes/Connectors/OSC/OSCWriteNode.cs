using System.Collections.Concurrent;
using Blazor.Diagrams.Core.Geometry;
using dOSC.Client.Engine.Ports;
using dOSC.Shared.Models.Wiresheet;
using dOSCEngine.Services;
using OSCService = dOSC.Client.Services.Connectors.Hub.OSC.OSCService;

namespace dOSC.Client.Engine.Nodes.Connectors.OSC
{
    public class OSCWriteNode: BaseNode
    {
        public OSCWriteNode(Guid? guid = null, ConcurrentDictionary<EntityPropertyEnum, dynamic>? properties = null, Point? position = null, ServiceBundle? service = null) : base(guid, position, properties)
        {
            AddPort(new MultiPort(PortGuids.Port_1, this, true, name: "Set Value", allowedTypes: new() { PortType.Numeric, PortType.String, PortType.Logic, PortType.Multi }));
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
            VisualIndicator = OSCAddress ?? "No Address Set";
            VisualIndicator = string.IsNullOrEmpty(VisualIndicator) ? "No Address Set" : VisualIndicator;

        }
        public override string Name => "OSC Write Node";
        public override string Category => NodeCategoryType.Connector;
        public override string Icon => "icon-hard-drive-upload";
        public string OSCAddress;
        private bool _isAvatarParameter;
        private bool _writeAsFloat;
        private bool _sendChatMessageImmediately;
        private bool _sendChatMessageWithSound;
        private readonly OSCService? _service = null;

        public override void PropertyNotifyEvent(EntityPropertyEnum property, dynamic? value)
        {
            if(property == EntityPropertyEnum.OSCAddress)
            {
                OSCAddress = value;
                VisualIndicator = OSCAddress ?? "No Address Set";
                VisualIndicator = string.IsNullOrEmpty(VisualIndicator) ? "No Address Set" : VisualIndicator;

            }
            else if(property == EntityPropertyEnum.IsAvatarParameter)
            {
                _isAvatarParameter = value;
            }
            else if(property == EntityPropertyEnum.WriteAsFloat)
            {
                _writeAsFloat = value;
            }
            else if(property == EntityPropertyEnum.SendChatMessageImmediately)
            {
                _sendChatMessageImmediately = value;
            }
            else if(property == EntityPropertyEnum.SendChatMessageWithSound)
            {
                _sendChatMessageWithSound = value;
            }
        }

        private object _updateLock = new();
        
        public override void CalculateValue()
        {
            lock (_updateLock)
            {
                if (_service != null)
                {
                    if (!string.IsNullOrEmpty(OSCAddress))
                    {
                        BasePort? input = Ports.First() as BasePort;
                        if(input?.HasValidLinks() ?? false)
                        {
                            var linkValue = input.GetInputValue();
                            if(linkValue != null)
                            {
                                PortType? portType = GetCurrentMultiPortType();
                                if (portType.HasValue)
                                {
                                    if (linkValue == null) return;
                                    switch (portType)
                                    {
                                        case PortType.Numeric:
                                            SendNumeric(GetFullAddress(), linkValue, _writeAsFloat);
                                            break;
                                        case PortType.String:
                                            SendChatMessage(linkValue, _sendChatMessageWithSound, _sendChatMessageImmediately);
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
            
        }

        private string GetFullAddress() => string.Join('/', $"{(_isAvatarParameter ? "/avatar/parameters" : string.Empty)}",OSCAddress);

        private void SendNumeric(string Address, dynamic Value, bool WriteAsFloat = false)
        {
            if (WriteAsFloat)
                Value = System.Math.Clamp(Convert.ToInt32(Value), -1, 1);
            else
                Value = System.Math.Clamp(Convert.ToInt32(Value), 0, 255);
            if (_service != null)
            {
                _service.SendMessage(Address, Value);
            }
        }

        private void SendBool(string Address, dynamic Value)
        {
            Value = System.Math.Clamp(Convert.ToInt32(Value), 0, 1);
            if (_service != null)
            {
                _service.SendMessage(Address, Value);
            }
        }

        private void SendChatMessage(dynamic Value, bool UseSound = false, bool QuickMessage = false)
        {
            if (_service != null)
            {
                _service.SendChatMessage(Value, QuickMessage, UseSound);
            }

        }

        public override void OnDispose()
        {
            UnsubscribeToAllPortTypeChanged();
            base.OnDispose();
        }
    }
}
