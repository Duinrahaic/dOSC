using dOSC.Client.Engine.Nodes.Variables;
using Microsoft.AspNetCore.Components;

namespace dOSC.Client.Components.Inputs
{
    public partial class ToggleSwitch   
    {
        [Parameter]
        public LogicNode Node { get; set; }
        [Parameter]
        public string Text { get; set; } = string.Empty;
        [Parameter]
        public bool Disabled { get; set; } = false;

        private bool _value;
        private bool Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
                Node.Value = value;
            }
        }

        protected override void OnParametersSet()
        {
            _value = Node.Value;
            StateHasChanged();
        }
    }
}
