using Blazor.Diagrams.Core.Geometry;
using dOSCEngine.Services.Connectors.OSC;
using dOSCEngine.Engine.Ports;
using Newtonsoft.Json;

namespace dOSCEngine.Engine.Nodes.Connector.VRChat
{
    public class OSCVRCAvatarIntReadNode : BaseNode, IDisposable
    {
        public OSCVRCAvatarIntReadNode(string SelectedOption = OSCService.Face, OSCService? service = null, Point? position = null) : base(position ?? new Point(0, 0))
        {
            AddPort(new NumericPort(PortGuids.PortGuidGenerator(1), this, false));
            _service = service;
            _service.OnOSCMessageRecieved += OnMessageReceived;

            this.SelectedOption = SelectedOption;
        }
        public OSCVRCAvatarIntReadNode(Guid guid, string SelectedOption = OSCService.Face, OSCService? service = null, Point? position = null) : base(guid, position ?? new Point(0, 0))
        {
            AddPort(new NumericPort(PortGuids.PortGuidGenerator(1), this, false));
            _service = service;
            _service.OnOSCMessageRecieved += OnMessageReceived;

            this.SelectedOption = SelectedOption;
        }

 

        [JsonProperty]
        public override string NodeClass => GetType().Name.ToString();
        private readonly OSCService? _service = null;
        [JsonProperty]
        public override string Option => SelectedOption;
        public string SelectedOption = OSCService.Face;
        public string SelectedOptionText => Options.FirstOrDefault(x => x.Key == SelectedOption).Value;
        public override string BlockTypeClass => "connectorblock";

        public void SetOption(string option)
        {
            SelectedOption = option;
        }
        public Dictionary<string, string> Options = new(){
            { OSCService.Face ,"Face"},
            { OSCService.Viseme ,"Viseme"},
            { OSCService.TrackingType ,"Tracking Type"},
            { OSCService.GestureLeft ,"Gesture Left"},
            { OSCService.GestureRight ,"Gesture Right"},
        };

        private void OnMessageReceived(OSCSubscriptionEvent e)
        {
            if (SelectedOption != null)
            {
                if (e.Address.ToLower() == SelectedOption.ToLower())
                {
                    var val = Convert.ToInt32(e.Arguments.First());
                    Value = System.Math.Clamp(val, 0, 255);
                }
            }
        }

        public void Dispose()
        {
            _service.OnOSCMessageRecieved -= OnMessageReceived;
        }
    }
}
