using Blazor.Diagrams.Core.Geometry;
using dOSCEngine.Services.Connectors.OSC;
using dOSCEngine.Engine.Ports;
using Newtonsoft.Json;
using dOSCEngine.Services;

namespace dOSCEngine.Engine.Nodes.Connectors.VRChat
{
    public class OSCVRCAvatarFloatReadNode : BaseNode, IDisposable
    {
        public OSCVRCAvatarFloatReadNode(string SelectedOption = OSCService.VelocityZ, ServiceBundle? service = null, Point? position = null) : base(position ?? new Point(0, 0))
        {
            AddPort(new NumericPort(PortGuids.PortGuidGenerator(1), this, false));
            _service = service?.OSC;
            _service.OnOSCMessageRecieved += OnMessageReceived;

            this.SelectedOption = SelectedOption;
        }
        public OSCVRCAvatarFloatReadNode(Guid guid, string SelectedOption = OSCService.VelocityZ, ServiceBundle? service = null, Point? position = null) : base(guid, position ?? new Point(0, 0))
        {
            AddPort(new NumericPort(PortGuids.PortGuidGenerator(1), this, false));
            _service = service?.OSC;
            _service.OnOSCMessageRecieved += OnMessageReceived;

            this.SelectedOption = SelectedOption;
        }

        [JsonProperty]
        public override string NodeClass => GetType().Name.ToString();
        private readonly OSCService? _service = null;
        [JsonProperty]
        public override string Option => SelectedOption;
        public string SelectedOption = OSCService.VelocityZ;
        public string SelectedOptionText => Options.FirstOrDefault(x => x.Key == SelectedOption).Value;
        public override string BlockTypeClass => "connectorblock";

        public void SetOption(string option)
        {
            SelectedOption = option;
        }
        public Dictionary<string, string> Options = new(){
            { OSCService.VelocityZ ,"Velocity Z"},
            { OSCService.VelocityX ,"Velocity X"},
            { OSCService.VelocityY ,"Velocity Y"},
            { OSCService.Upright ,"Upright"},
            { OSCService.AngularY ,"Angular Y"},
            { OSCService.GestureLeftWeight ,"Gesture Left Weight"},
            { OSCService.GestureRightWeight ,"Gesture Right Weight"},
        };

        private void OnMessageReceived(OSCSubscriptionEvent e)
        {
            if (SelectedOption != null)
            {
                if (e.Address.ToLower() == SelectedOption.ToLower())
                {
                    var val = Convert.ToDouble(e.Arguments.First());
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
