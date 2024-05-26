using Microsoft.AspNetCore.Components;

namespace dOSC.Component.Modals;

public partial class ModalBase
{
    private string modalClass = "";
    private string modalDisplay = "none;";
    private bool showBackdrop;

    [Parameter] public RenderFragment Title { get; set; }

    [Parameter] public RenderFragment Body { get; set; }

    [Parameter] public RenderFragment Footer { get; set; }

    public void Open()
    {
        modalDisplay = "flex;";
        modalClass = "show";
        showBackdrop = true;
    }

    public void Close()
    {
        modalDisplay = "none";
        modalClass = "";
        showBackdrop = false;
    }
}