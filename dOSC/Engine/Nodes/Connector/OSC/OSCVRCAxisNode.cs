﻿using Blazor.Diagrams.Core.Geometry;
using dOSC.Engine.Ports;
using dOSC.Services.Connectors.OSC;

namespace dOSC.Engine.Nodes.Connector.OSC
{
    public class OSCVRCAxisNode : BaseNode
    {
        public OSCVRCAxisNode(string SelectedOption = OSCService.Vertical, OSCService ? service = null, Point? position = null) : base(position ?? new Point(0, 0))
        {
            foreach(var _ in Options)
            {
                AddPort(new NumericPort(this, true));
            }
            _service = service;
            this.SelectedOption = SelectedOption;
        }
        private readonly OSCService? _service = null;
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