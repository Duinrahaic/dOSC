using dOSC.Client.Engine.Nodes;
using Microsoft.AspNetCore.Components;
using ServiceBundle = dOSC.Client.Services.ServiceBundle;

namespace dOSC.Client.Components.Modals;

public partial class EditorModal : IDisposable
{
    private ModalV2? Modal;
    private WiresheetNode? SelectedBaseNode;
    [Inject] private ServiceBundle? sb { get; set; }


    public void Dispose()
    {
        if (sb != null) sb.OnNodeEdit -= OnEdit;
    }

    protected override void OnInitialized()
    {
        if (sb != null) sb.OnNodeEdit += OnEdit;
    }

    private void OnEdit(BaseNode node)
    {
        SelectedBaseNode = node;
        if (SelectedBaseNode != null && Modal != null)
        {
            Modal.Open();
            InvokeAsync(() => { StateHasChanged(); }).ConfigureAwait(false);
        }
    }


    private void Updated()
    {
        Modal?.Close();
    }
}