using Microsoft.AspNetCore.Components;

namespace dOSC.Client.Engine.Helper;

public partial class PortValueLabel
{
    public enum Position
    {
        Left,
        Right
    }

    private DateTime _lastUpdate = DateTime.MinValue;

    [Parameter] public dynamic? DisplayValue { get; set; }

    [Parameter] public Position LabelPosition { get; set; } = Position.Right;

    [Parameter] public string StringFormat { get; set; } = "G5";


    private string LabelPositionClass => LabelPosition switch
    {
        Position.Left => "left",
        Position.Right => "right",
        _ => "right"
    };

    protected override void OnParametersSet()
    {
        Update();
    }

    private void Update()
    {
        if (DateTime.Now - _lastUpdate > GraphSettings.UpdateInterval)
        {
            _lastUpdate = DateTime.Now;
            InvokeAsync(StateHasChanged);
        }
    }
}