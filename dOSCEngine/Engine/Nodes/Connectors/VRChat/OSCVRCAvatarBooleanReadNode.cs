using Blazor.Diagrams.Core.Geometry;
using dOSCEngine.Services.Connectors.OSC;
using dOSCEngine.Engine.Ports;
using Newtonsoft.Json;
using dOSCEngine.Services;

namespace dOSCEngine.Engine.Nodes.Connectors.VRChat
{
    public class OSCVRCAvatarBooleanReadNode : BaseNode, IDisposable
    {
        public OSCVRCAvatarBooleanReadNode(string SelectedOption = OSCService.InStation, ServiceBundle? service = null, Point? position = null) : base(position ?? new Point(0, 0))
        {
            AddPort(new LogicPort(PortGuids.Port_1, this, false));
            _service = service?.OSC;
            _service.OnOSCMessageRecieved += OnMessageReceived;

            this.SelectedOption = SelectedOption;
        }
        public OSCVRCAvatarBooleanReadNode(Guid guid, string SelectedOption = OSCService.InStation, ServiceBundle? service = null, Point? position = null) : base(guid, position ?? new Point(0, 0))
        {
            AddPort(new LogicPort(PortGuids.Port_1, this, false));
            _service = service?.OSC;
            _service.OnOSCMessageRecieved += OnMessageReceived;

            this.SelectedOption = SelectedOption;
        }

        [JsonProperty]
        public override string NodeClass => GetType().Name.ToString();
        private readonly OSCService? _service = null;
        [JsonProperty]
        public override string Option => SelectedOption;
        public string SelectedOption = OSCService.InStation;
        public string SelectedOptionText => Options.FirstOrDefault(x => x.Key == SelectedOption).Value;
        public override string BlockTypeClass => "connectorblock";

        public void SetOption(string option)
        {
            SelectedOption = option;
        }
        public Dictionary<string, string> Options = new(){
            { OSCService.InStation ,"In Station"},
            { OSCService.Seated ,"Seated"},
            { OSCService.AFK ,"AFK"},
            { OSCService.MuteSelf ,"Mute Self"},
            { OSCService.GestureRight ,"Gesture Right"},
        };

        private void OnMessageReceived(OSCSubscriptionEvent e)
        {
            if (SelectedOption != null)
            {
                if (e.Address.ToLower() == SelectedOption.ToLower())
                {
                    var val = Convert.ToBoolean(e.Arguments.First());
                    Value = val;
                }
            }
        }

        public void Dispose()
        {
            _service.OnOSCMessageRecieved -= OnMessageReceived;
        }
    }
}
