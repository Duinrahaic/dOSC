using Microsoft.AspNetCore.Components;

namespace dOSC.Client.Components.Modals
{
    public partial class ModalBase
    {
        [Parameter]
        public RenderFragment Title { get; set; }

        [Parameter]
        public RenderFragment Body { get; set; }

        [Parameter]
        public RenderFragment Footer { get; set; }
        private string modalDisplay = "none;";
        private string modalClass = "";
        private bool showBackdrop = false;

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
}
