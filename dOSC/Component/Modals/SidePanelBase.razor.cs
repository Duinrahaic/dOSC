using dOSC.Component.Wiresheet;
using dOSC.Drivers;
using dOSC.Utilities;
using LiveSheet;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;

namespace dOSC.Component.Modals;

public partial class SidePanelBase
{
    private bool _closeAnimation;

    private ModalV2 _deleteModal = default!;

    private bool _show = false;

 

    [Inject] private WiresheetService Engine { get; set; } = default!;

    [Inject] private IJSRuntime JS { get; set; }= default!;

    [Parameter] public WiresheetDiagram App { get; set; }

    private string ReplacementImage64 { get; set; } = string.Empty;
    [Parameter] public EventCallback<WiresheetDiagram> OnUpdate { get; set; }

    private byte[] ImgUpload { get; set; } = Array.Empty<byte>();

    public void Open()
    {
        ReplacementImage64 = "";
        _show = true;
        StateHasChanged();
    }

    public async Task Close()
    {
        ReplacementImage64 = "";
        _closeAnimation = true;
        await Task.Delay(250);
        _show = false;
        _closeAnimation = false;
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
        if (!string.IsNullOrEmpty(ReplacementImage64)) App.AppIcon = ReplacementImage64;
        
        await Close();
        await OnUpdate.InvokeAsync(App);
    }


    private async Task ClearAppImage()
    {
        App.AppIcon = string.Empty;
        await OnUpdate.InvokeAsync(App);
    }

    private void OnDeleteApp()
    {
        _deleteModal.Open();
    }

    private async Task OnDeleteConfirmed()
    {
        _deleteModal.Close();
        await Close();
        Engine.RemoveApp(App);
        await OnUpdate.InvokeAsync(App);
    }


    private async Task DownloadApp()
    {
        await JS.DownloadApp(App);
    }
}