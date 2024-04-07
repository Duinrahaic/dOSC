using dOSC.Client.Engine.Nodes.Variables;
using Microsoft.AspNetCore.Components;

namespace dOSC.Client.Components.Inputs;

public partial class ToggleSwitch
{
    private bool _value;

    [Parameter] public LogicNode Node { get; set; }

    [Parameter] public string Text { get; set; } = string.Empty;

    [Parameter] public bool Disabled { get; set; }

    private bool Value
    {
        get => _value;
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