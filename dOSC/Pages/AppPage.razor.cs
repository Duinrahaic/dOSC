using dOSC.Component.Modals;
using dOSC.Component.UI.App;
using dOSC.Component.Wiresheet;
using dOSC.Drivers;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace dOSC.Pages;

public partial class AppPage
{
    private string ActiveTab = "All";


    private List<WiresheetDiagram> _apps = new();
    private SidePanelBase _appSettingsPanel = default!;
    private bool _hasFile = false ;

    private List<AppFilterType> _navTabs = Enum.GetValues(typeof(AppFilterType)).Cast<AppFilterType>().ToList();
    private WiresheetDiagram? _selectedApp;


    // Modals 
    private ModalV2 _uploadAppModal = default!;

    private WiresheetDiagram? _uploadedFile = null;
    
    [Inject] public WiresheetService Engine { get; set; } = default!;

    [Inject] public IJSRuntime Js { get; set; } = default!;

    [Inject] public NavigationManager Nm { get; set; }  = default!;

    [Parameter] public string AppId { get; set; } = string.Empty;


    protected override void OnInitialized()
    {
        _apps = Engine.GetApps();
        
        _apps.Add(new()
        {
            Name = "Test"
        });
        
        _selectedApp = _apps.FirstOrDefault();
    }

    protected override void OnParametersSet()
    {
        if (!string.IsNullOrEmpty(AppId)) _selectedApp = Engine.GetAppById(AppId);
    }


    private void UploadApp()
    {
        if (_uploadedFile != null)
        {
            _uploadAppModal.Close();
            Engine.AddApp(_uploadedFile);
            _apps = Engine.GetApps();
            _selectedApp = _apps.FirstOrDefault();
        }

        _hasFile = false;
    }

    private void UploadedFileCallback(WiresheetDiagram? upload )
    {
        if (upload == null)
        {
            _hasFile = false;
        }
        else
        {
            _hasFile = true;
            _uploadedFile = upload;
        }
    }


    private void NewApp()
    {
        Nm.NavigateTo("/editor");
    }

    private void ShowSettings(WiresheetDiagram? app)
    {
        if (app != null)
        {
            _selectedApp = app;
            _appSettingsPanel?.Open();
        }
    }

    private void Save(WiresheetDiagram app)
    {
        Engine.UpdateApp(app); 
    }
}