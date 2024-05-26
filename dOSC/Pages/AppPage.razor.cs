using dOSC.Client.Components.Modals;
using dOSC.Client.Engine;
using dOSC.Client.Services;
using dOSC.Drivers;
using dOSC.Shared.Models.Wiresheet;
using LiveSheet;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace dOSC.Pages;

public partial class AppPage
{
    private string ActiveTab = "All";


    private List<LiveSheetDiagram> Apps = new();
    private SidePanelBase AppSettingsPanel;
    private bool HasFile;

    private List<string> NavTabs = new() { "All", "Activity", "Fun", "Tech" };
    private LiveSheetDiagram? SelectedApp;


    // Modals 
    private ModalV2 UploadAppModal;

    private dOSCDataDTO? UploadedFile;

    [Inject] public WiresheetService Engine { get; set; } = default!;

    [Inject] public IJSRuntime JS { get; set; } = default!;

    [Inject] public NavigationManager NM { get; set; }  = default!;

    [Parameter] public Guid? AppId { get; set; }


    protected override void OnInitialized()
    {
        Apps = Engine?.GetApps() ?? new List<LiveSheetDiagram>();
        SelectedApp = Apps.FirstOrDefault();
    }

    protected override void OnParametersSet()
    {
        if (Engine == null) return;
        if (AppId.HasValue) SelectedApp = Engine.GetAppByID(AppId.Value);
    }


    private void UploadApp()
    {
        if (UploadedFile != null)
        {
            UploadAppModal.Close();
            var ws = UploadedFile.DeserializeDTO(Engine.ServiceBundle);
            if (ws != null)
            {
                var UploadedApp = new AppLogic(ws);
                Engine.AddApp(UploadedApp);

                try
                {
                    Engine.AddApp(UploadedApp);
                }
                catch
                {
                }

                Apps = Engine.GetApps();
                SelectedApp = Apps.FirstOrDefault();
            }
        }

        HasFile = false;
    }

    private void UploadedFileCallback(dOSCDataDTO? Upload)
    {
        if (Upload == null)
        {
            HasFile = false;
        }
        else
        {
            HasFile = true;
            UploadedFile = Upload;
        }
    }


    private void NewApp()
    {
        if (NM != null) NM.NavigateTo("/editor");
    }

    private void ShowSettings(AppLogic app)
    {
        if (app != null)
        {
            SelectedApp = app;
            AppSettingsPanel.Open();
        }
    }

    private void Save(AppLogic app)
    {
        if (app != null && Engine != null) Engine.UpdateApp(app);
    }

    private void OnUpdated(AppLogic appLogic)
    {
        StateHasChanged();
    }
}