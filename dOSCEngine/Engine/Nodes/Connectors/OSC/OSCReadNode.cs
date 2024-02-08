using dOSCEngine.Engine.Ports;
using dOSCEngine.Services.Connectors.OSC;
using dOSCEngine.Services;
using dOSCEngine.Utilities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blazor.Diagrams.Core.Geometry;

namespace dOSCEngine.Engine.Nodes.Connectors.OSC
{
    public class OSCReadNode : BaseNode
    {
        public OSCReadNode(Guid? guid = null, ConcurrentDictionary<EntityPropertyEnum, dynamic>? properties = null, Point? position = null, ServiceBundle? service = null) : base(guid, position, properties)
        {
            AddPort(new MultiPort(PortGuids.Port_1, this, false, limitLink: false, name: "Read Value", allowedTypes: new() { PortType.Numeric, PortType.Logic, PortType.Multi }));
            _service = service?.OSC;
            Properties.TryInitializeProperty(EntityPropertyEnum.IsAvatarParameter, true);
            Properties.TryInitializeProperty(EntityPropertyEnum.OSCAddress, string.Empty);
            Properties.TryInitializeProperty(EntityPropertyEnum.WriteAsFloat, true);

            OSCAddress = Properties.GetProperty<string>(EntityPropertyEnum.OSCAddress);
            _isAvatarParameter = Properties.GetProperty<bool>(EntityPropertyEnum.IsAvatarParameter);
            _writeAsFloat = Properties.GetProperty<bool>(EntityPropertyEnum.WriteAsFloat);
   
            if (_service != null)
                _service.OnOSCMessageRecieved += OnOSCMessageReceived;
            VisualIndicator = OSCAddress ?? "No Address Set";
            VisualIndicator = string.IsNullOrEmpty(VisualIndicator) ? "No Address Set" : VisualIndicator;
        }
        public override string Name => "OSC Read Node";
        public override string Category => NodeCategoryType.Connector;
        public override string Icon => "icon-hard-drive-download";
        public string OSCAddress;
        private bool _isAvatarParameter;
        private bool _writeAsFloat;
        private readonly OSCService? _service = null;
  
        private string GetFullAddress() => string.Join('/', $"{(_isAvatarParameter ? "/avatar/parameters" : string.Empty)}", OSCAddress);
        private void OnOSCMessageReceived(OSCSubscriptionEvent e)
        {
            if(!string.IsNullOrEmpty(OSCAddress))
            {
                if (e.Address.Equals(GetFullAddress(), StringComparison.OrdinalIgnoreCase))
                {
                    Value = GetNumeric((dynamic?)e.Arguments.FirstOrDefault(),_writeAsFloat);
                }
            }
            
        }
        
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
                if (Classifier.IsBooleanType(Value))
                {
                    value = System.Math.Clamp(Convert.ToBoolean(Value), 0, 1);
                }
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
}
