using Microsoft.AspNetCore.Components;

namespace dOSC.Client.Components.Modals
{
    public partial class ModalV2
    {
        [Parameter]
        public string Title { get; set; } = string.Empty;

        [Parameter]
        public RenderFragment Body { get; set; }

        [Parameter]
        public RenderFragment Footer { get; set; }

        [Parameter]
        public bool BackdropClose { get; set; } = true;

        [Parameter]
        public ModalSize? Size { get; set; }

        [Parameter]
        public EventCallback OnClose { get; set; }

        private bool Show = false;
        private bool Disappear = false;


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

        public enum ModalSize
        {
            Small,
            Medium,
            Large,
            Full
        }

        public void Open()
        {
            Show = true;
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
            if (BackdropClose)
            {
                Close();
            }

        }
    }
}
