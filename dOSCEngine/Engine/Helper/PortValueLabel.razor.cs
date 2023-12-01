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

        private bool IsNumeric => DisplayValue is int || DisplayValue is float || DisplayValue is double || DisplayValue is decimal;

        [Parameter]
        public Position LabelPosition { get; set; } = Position.Right;

        protected override void OnParametersSet()
        {
            this.StateHasChanged();
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
