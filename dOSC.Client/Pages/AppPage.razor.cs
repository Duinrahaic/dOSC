using dOSC.Client.Components.Modals;
using dOSC.Client.Engine;
using dOSC.Client.Services;
using dOSC.Shared.Models.Wiresheet;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using SidePanelBase = dOSC.Client.Components.Modals.SidePanelBase;

namespace dOSC.Client.Pages
{
    public partial class AppPage
    {

        [Inject]
        public dOSCService? Engine { get; set; }
        [Inject]
        public IJSRuntime? JS { get; set; }
        [Inject]
        public NavigationManager? NM { get; set; }

        [Parameter]
        public Guid? AppId { get; set; }

        
        private List<AppLogic> Apps = new();
        private AppLogic? SelectedApp;

        private List<string> NavTabs = new() { "All", "Activity", "Fun", "Tech" };
        private string ActiveTab = "All";
        private bool HasFile = false;


        protected override void OnInitialized()
        {
            Apps = Engine?.GetApps() ?? new();
            SelectedApp = Apps.FirstOrDefault();
        }

        protected override void OnParametersSet()
        {
            if (Engine == null) return;
            if (AppId.HasValue)
            {
                SelectedApp = Engine.GetAppByID(AppId.Value);
            }
        }

      



        private void UploadApp()
        {
            if(UploadedFile != null)
            {
                UploadAppModal.Close();
                dOSCData? ws = UploadedFile.DeserializeDTO(Engine.ServiceBundle);
                if(ws != null)
                {
                    AppLogic UploadedApp = new AppLogic(ws, AppState.Disabled, AutomationState.Disabled);
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

        private dOSCDataDTO? UploadedFile;
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
            if (NM != null)
            {
                NM.NavigateTo($"/editor");
            }
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
            if (app != null && Engine != null)
            {
                Engine.UpdateApp(app);
            }
        }

        private void OnUpdated(AppLogic appLogic)
        {
            this.StateHasChanged();
        }
        

        // Modals 
        private ModalV2 UploadAppModal { get; set; }
        private SidePanelBase AppSettingsPanel { get; set; }
    }
}
