using Blazor.Diagrams.Core;
using Microsoft.AspNetCore.Components;
using Blazor.Diagrams.Components.Renderers;
using Blazor.Diagrams.Core.Models;
using dOSC.Engine.Nodes.Math;
using dOSC.Engine.Nodes.Constant;

namespace dOSC.Components.Inputs
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
    }
}
