using Blazor.Diagrams.Core.Geometry;
using dOSCEngine.Services.Connectors.OSC;
using dOSCEngine.Engine.Ports;
using Newtonsoft.Json;
using dOSCEngine.Services;

namespace dOSCEngine.Engine.Nodes.Connectors.VRChat
{
    public class OSCVRCAxisNode : BaseNode
    {
        public OSCVRCAxisNode(string SelectedOption = OSCService.Vertical, ServiceBundle? service = null, Point? position = null) : base(position ?? new Point(0, 0))
        {
            int portNumber = 1;
            foreach (var _ in Options)
            {
                AddPort(new NumericPort(PortGuids.PortGuidGenerator(portNumber), this, true));
                portNumber++;
            }
            _service = service?.OSC;
            this.SelectedOption = SelectedOption;
        }
        public OSCVRCAxisNode(Guid guid, string SelectedOption = OSCService.Vertical, ServiceBundle? service = null, Point? position = null) : base(guid, position ?? new Point(0, 0))
        {
            int portNumber = 1;
            foreach (var _ in Options)
            {
                AddPort(new NumericPort(PortGuids.PortGuidGenerator(portNumber), this, true));
                portNumber++;
            }
            _service = service?.OSC;
            this.SelectedOption = SelectedOption;
        }

        [JsonProperty]
        public override string NodeClass => GetType().Name.ToString();
        private readonly OSCService? _service = null;
        [JsonProperty]
        public override string Option => SelectedOption;
        public string SelectedOption = OSCService.Vertical;
        public string SelectedOptionText => Options.FirstOrDefault(x => x.Key == SelectedOption).Value;
        public override string BlockTypeClass => "connectorblock";

        public void SetOption(string option)
        {
            SelectedOption = option;
        }
        public Dictionary<string, string> Options = new(){
            { OSCService.Vertical ,"Vertical"},
            { OSCService.Horizontal ,"Horizontal"},
            { OSCService.LookHorizontal ,"Look Horizontal"},
            { OSCService.LookVertical ,"Look Vertical"},
            { OSCService.UseAxisRight ,"Use Axis Right"},
            { OSCService.GrabAxisRight ,"Grab Axis Right"},
            { OSCService.MoveHoldFB ,"Move Forward/Backwards"},
            { OSCService.SpinHoldCwCcw ,"Spin Object CW/CC"},
            { OSCService.SpinHoldUD ,"Spin Object Up/Down"},
            { OSCService.SpinHoldLR ,"Spin Object Left/Right"},
        };

        public override void CalculateValue()
        {
            if (_service != null)
            {
                var input = Ports.First();
                if (input.Links.Any())
                {
                    var i = GetInputValue(input, input.Links.First());
                    var v = Convert.ToInt32(i);
                    _service.SendMessage(SelectedOption, v);
                    Value = v;
                }
            }
        }
    }
}
