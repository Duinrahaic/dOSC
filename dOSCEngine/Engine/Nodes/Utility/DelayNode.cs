using Blazor.Diagrams.Core.Geometry;
using dOSCEngine.Engine.Ports;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dOSCEngine.Engine.Nodes.Utility
{
    public class DelayNode : BaseNode
    {
        public DelayNode(string? SelectedOption = null, Point? position = null) : base(position ?? new Point(0, 0))
        {
            AddPort(new MultiPort(PortGuids.Port_1, this, true));
            AddPort(new MultiPort(PortGuids.Port_2, this, false));
            int.TryParse(SelectedOption, out int result);
            this.SelectedOption = result == 0? _DefaultValue : result;

        }
        public DelayNode(Guid guid, string? SelectedOption = null, Point? position = null) : base(guid, position ?? new Point(0, 0))
        {
            AddPort(new MultiPort(PortGuids.Port_1, this, true));
            AddPort(new MultiPort(PortGuids.Port_2, this, false));
            int.TryParse(SelectedOption, out int result);
            this.SelectedOption = result == 0 ? _DefaultValue : result;
        }
        [JsonProperty]
        public override string NodeClass => GetType().Name.ToString();
        public override string BlockTypeClass => "numericblock";
        [JsonProperty]
        public override string Option => SelectedOption.ToString();

        private static int _DefaultValue => 1000; // ms

        [Range(100,int.MaxValue)]
        private int _SelectedOption { get; set; } = _DefaultValue; //ms
        public int SelectedOption
        {
            get => _SelectedOption;
            set
            {
                if (Value != null)
                {
                    if(value >= 100)
                    {
                        _SelectedOption = value;
                    }
                    else
                    {
                        _SelectedOption = _DefaultValue;
                    }
                }
                else
                {
                    _SelectedOption = _DefaultValue;
                }
            }
        }

        private bool Delaying = false;

        public override void CalculateValue()
        {
            if(Delaying == false)
            {
                var Input = Ports[0];
                if (Input != null)
                {
                    if (Input.Links.Any())
                    {
                        var Value = GetInputValue(Input, Input.Links.First());
                        if (Value != null)
                        {
                            Delaying = true;
                            DelayUpdate(Value);
                        }
                    }
                }
            }
            else
            {
            }
        }

        private async void DelayUpdate(dynamic? Value)
        {
            await Task.Delay(SelectedOption);
            Delaying = false;
            this.Value = Value;
        }
    }
}
