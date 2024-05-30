using Microsoft.AspNetCore.Components;

namespace dOSC.Component.UI.Wiresheet;

public partial class PlayPauseButton
{
    [Parameter] public bool IsPlaying { get; set; }

    [Parameter] public EventCallback<bool> IsPlayingChanged { get; set; }

    private void Click()
    {
        IsPlaying = !IsPlaying;
        IsPlayingChanged.InvokeAsync(IsPlaying);
    }
}