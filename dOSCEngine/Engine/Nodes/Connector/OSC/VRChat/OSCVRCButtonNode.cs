using Blazor.Diagrams.Core.Geometry;
using dOSCEngine.Services.Connectors.OSC;
using dOSCEngine.Engine.Nodes;
using dOSCEngine.Engine.Ports;
using Newtonsoft.Json;

namespace dOSCEngine.Engine.Nodes.Connector.OSC.VRChat
{
    public class OSCVRCButtonNode : BaseNode
    {
        public OSCVRCButtonNode(string SelectedOption = OSCService.MoveForward, OSCService? service = null, Point? position = null) : base(position ?? new Point(0, 0))
        {
            AddPort(new LogicPort(PortGuids.Port_1, this, true));
            _service = service;
            this.SelectedOption = SelectedOption;
        }
        public OSCVRCButtonNode(Guid guid, string SelectedOption = OSCService.MoveForward, OSCService? service = null, Point? position = null) : base(guid, position ?? new Point(0, 0))
        {
            AddPort(new LogicPort(PortGuids.Port_1, this, true));
            _service = service;
            this.SelectedOption = SelectedOption;
        }
        [JsonProperty]
        public override string NodeClass => GetType().Name.ToString();
        private readonly OSCService? _service = null;
        [JsonProperty]
        public override string Option => SelectedOption;
        public string SelectedOption = OSCService.MoveForward;
        public string SelectedOptionText => Options.FirstOrDefault(x => x.Key == SelectedOption).Value;
        public override string BlockTypeClass => "connectorblock";

        public void SetOption(string option)
        {
            SelectedOption = option;
        }

        public Dictionary<string, string> Options = new(){
            { OSCService.MoveForward ,"Move Forward"},
            { OSCService.MoveBackward ,"Move Backwards"},
            { OSCService.MoveLeft ,"Move Left"},
            { OSCService.MoveRight ,"Move Right"},
            { OSCService.LookLeft ,"Look Left"},
            { OSCService.LookRight ,"Look Right"},
            { OSCService.LookDown ,"Look Down"},
            { OSCService.LookUp ,"Look Up"},
            { OSCService.Jump ,"Jump"},
            { OSCService.Run ,"Back"},
            { OSCService.Menu ,"Menu"},
            { OSCService.ComfortLeft ,"Comfort Left"},
            { OSCService.ComfortRight ,"Comfort Right"},
            { OSCService.DropRight, "Drop Right"},
            { OSCService.GrabRight, "Grab Right"},
            { OSCService.UseRight, "Use Right"},
            { OSCService.DropLeft, "Drop Left"},
            { OSCService.GrabLeft, "Grab Left"},
            { OSCService.UseLeft, "Use Left"},
            { OSCService.PanicButton, "Panic Button"},
            { OSCService.QuickMenuToggleLeft, "Quick Menu Toggle Left"},
            { OSCService.QuickMenuToggleRight, "Quick Menu Toggle Right"},
            { OSCService.ToggleSitStand, "Toggle Sit/Stand"},
            { OSCService.AFKToggle, "AFK Toggle"},
            { OSCService.Voice, "Voice"},
        };

        public override void Refresh()
        {
            if (_service != null)
            {
                var input = Ports.First();
                if (input.Links.Any())
                {
                    var i = GetInputValue(input, input.Links.First());
                    var v = Convert.ToInt32(i);
                    _service.SendMessage(SelectedOption, v);
                }
            }
            base.Refresh();
        }
    }
}
