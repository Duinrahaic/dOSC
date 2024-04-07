using Microsoft.AspNetCore.Components;

namespace dOSC.Client.Components.Wiresheet;

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