using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dOSCEngine.Engine.Helper
{
    public partial class PortValueLabel
    {
        [Parameter]
        public dynamic? DisplayValue { get; set; }

        [Parameter]
        public Position LabelPosition { get; set; } = Position.Right;
        [Parameter]
        public string StringFormat { get; set; } = "G5";

        protected override void OnParametersSet()
        {
            Update();
        }
        private DateTime _lastUpdate = DateTime.MinValue;
    
        private void Update()
        {
            if (DateTime.Now - _lastUpdate > GraphSettings.UpdateInterval)
            {
                _lastUpdate = DateTime.Now;
                InvokeAsync(StateHasChanged);
            }
        }
        
        
        private string LabelPositionClass => LabelPosition switch
        {
            Position.Left => "left",
            Position.Right => "right",
            _ => "right"
        };
        public enum Position
        {
            Left,
            Right
        }
    }
}
