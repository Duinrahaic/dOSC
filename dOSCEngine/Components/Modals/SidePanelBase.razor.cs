using dOSCEngine.Engine;
using dOSCEngine.Services;
using dOSCEngine.Utilities;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;



namespace dOSCEngine.Components.Modals
{
    public partial class SidePanelBase
    {
        [Inject]
        private dOSCService? Engine { get; set; }
        [Inject]
        private IJSRuntime? JS { get; set; }

        [Parameter]
        public AppLogic App { get; set; }
        private string ReplacementImage64 { get; set; } = string.Empty;
        public string CurrentAppIcon
        {
            get
            {
                if (App.Data == null)
                {
                    return AppDefaults.GetDefaultAppImage();
                }
                else
                {
                    if (!string.IsNullOrEmpty(ReplacementImage64))
                    {
                        return ReplacementImage64;
                    }
                    else
                    {
                        return App.Data.CurrentAppIcon;
                    }
                }
            }
        }

        private ModalV2? DeleteModal;

        [Parameter]
        public EventCallback<AppLogic> OnUpdate { get; set; }

        private bool Show = false;
        private bool CloseAnimation = false;

        private byte[] ImgUpload { get; set; }

        public void Open()
        {
            ReplacementImage64 = "";
            Show = true;
            StateHasChanged();
        }
        public async Task Close()
        {
            ReplacementImage64 = "";
            CloseAnimation = true;
            await Task.Delay(250);
            Show = false;
            CloseAnimation = false;
        }

        private async Task LoadFiles(InputFileChangeEventArgs e)
        {
            var format = "image/png";
            var image = e.GetMultipleFiles(1).FirstOrDefault(file => file.ContentType.StartsWith("image/"));
            try
            {
                var resizedImageFile = await image.RequestImageFileAsync(format, 240, 240);


                var buffer = new byte[resizedImageFile.Size];
                await resizedImageFile.OpenReadStream().ReadAsync(buffer);


                ReplacementImage64 = $"data:{format};base64,{Convert.ToBase64String(buffer, 0, buffer.Length)}";
            }
            catch (Exception ex)
            {
                ReplacementImage64 = string.Empty;
            }
            StateHasChanged();
        }

        private async Task OnValidSubmit(EditContext context)
        {
            if (!context.Validate())
                return;
            if (App.Data != null)
            {
                if (!string.IsNullOrEmpty(ReplacementImage64))
                {
                    App.SetAppIcon(ReplacementImage64);
                }
                App.Save();

                await Close();
                await OnUpdate.InvokeAsync(App);
            }
        }




        private async Task ClearAppImage()
        {
            if (App != null)
            {
                App.ResetAppIcon();
                await OnUpdate.InvokeAsync(App);
            }
        }

        private async Task OnDeleteApp()
        {
            DeleteModal.Open();
        }

        private async Task OnDeleteConfirmed()
        {
            DeleteModal.Close();
            await Close();
            Engine.RemoveApp(App);
            await OnUpdate.InvokeAsync(App);
        }


        private async Task DownloadApp()
        {
            if (App != null)
            {

                await JS.DownloadApp(App);
            }

        }
    }
}
