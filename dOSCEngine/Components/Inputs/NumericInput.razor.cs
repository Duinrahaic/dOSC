using Blazor.Diagrams.Core.Models;
using Microsoft.AspNetCore.Components;
using dOSCEngine.Engine.Nodes;
using dOSCEngine.Engine.Nodes.Constant;

namespace dOSCEngine.Components.Inputs
{
    public partial class NumericInput
    {
        [Parameter]
        public NumericNode? Node { get; set; }
        [Parameter]
        public string Text { get; set; } = string.Empty;
        [Parameter]
        public double Step { get; set; } = 1;
        [Parameter]
        public double Max { get; set; } = double.MaxValue;
        [Parameter]
        public double Min { get; set; } = double.MinValue;
        [Parameter]
        public int PortNumber { get; set; } = 0;


        private double _value;
        private double Value
        {
            get
            {
                return _value;
            }
            set
            {
                if (value > Max || value < Min) return;
                _value = value;
                if (Node != null)
                {
                    Node.Value = value;
                }
            }
        }

        private void Increment() => Value += Step;
        private void Decrement() => Value -= Step;
    }
}
