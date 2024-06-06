
using Microsoft.AspNetCore.Components;

namespace dOSC.Component.Modals;

public partial class ModalV2
{
    public enum ModalSize
    {
        Small,
        Medium,
        Large,
        Full
    }

    private bool Disappear = false;

    private bool Show = false;

    [Parameter] public string Title { get; set; } = string.Empty;

    [Parameter] public RenderFragment Body { get; set; }

    [Parameter] public RenderFragment Footer { get; set; }

    [Parameter] public bool BackdropClose { get; set; } = true;

    [Parameter] public ModalSize? Size { get; set; }

    [Parameter] public EventCallback OnClose { get; set; }

    [Parameter] public string Style { get; set; } = string.Empty;

    private string GetSize()
    {
        switch (Size)
        {
            case ModalSize.Small:
                return "dosc-modal-small";
            case ModalSize.Medium:
                return "dosc-modal-medium";
            case ModalSize.Large:
                return "dosc-modal-large";
            case ModalSize.Full:
                return "dosc-modal-full";
            default:
                return "dosc-modal-medium";
        }
    }

    public void Open()
    {
        Show = true;
        StateHasChanged();
    }

    public void Close()
    {
        Disappear = true;
        Task.Delay(50);
        Show = false;
        OnClose.InvokeAsync();
    }

    public void BackdropClick()
    {
        if (BackdropClose) Close();
    }
}