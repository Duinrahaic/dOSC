using dOSC.Components.Modals;
using dOSCEngine.Services;
using dOSCEngine.Utilities;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.IO;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace dOSC.Pages
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

        private List<dOSCWiresheet> Wiresheets = new();
        private dOSCWiresheet? Wiresheet;

        private List<string> NavTabs = new() { "All", "Activity", "Fun", "Tech" };
        private string ActiveTab = "All";
        private bool HasFile = false;


        protected override void OnInitialized()
        {
            Wiresheets = Engine?.GetWireSheets() ?? new();
            Wiresheet = Wiresheets.FirstOrDefault();
        }

        protected override void OnParametersSet()
        {
            if (Engine == null) return;
            if (AppId.HasValue)
            {
                Wiresheet = Engine.GetWiresheet(AppId.Value);
            }
        }

        private void OnSelected(dOSCWiresheet wiresheet)
        {
            Wiresheet = wiresheet;
        }



        private void UploadApp()
        {
            if(UploadedFile != null)
            {
                UploadAppModal.Close();
                dOSCWiresheet? wiresheet = Engine.DeserializeDTO(UploadedFile);
                Engine.SaveWiresheet(wiresheet);

                try
                {
                    Engine.AddWiresheet(wiresheet);
                }
                catch
                {
                
                }
                Wiresheets = Engine.GetWireSheets();
                OnSelected(wiresheet);
            }
            HasFile = false;
        }

        private dOSCWiresheetDTO? UploadedFile;
        private void UploadedFileCallback(dOSCWiresheetDTO? Upload)
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
                NM.NavigateTo($"apps/editor/");
            }
        }
    
        private void ShowSettings(dOSCWiresheet wiresheet)
        {
            if (wiresheet != null)
            {
                Wiresheet = wiresheet;
                AppSettingsPanel.Open();
            }

        }

        private void Save(dOSCWiresheet wiresheet)
        {
            if (wiresheet != null && Engine != null)
            {
                Engine.SaveWiresheet(wiresheet);
            }
        }

        private void OnUpdated(dOSCWiresheet wiresheet)
        {
            this.StateHasChanged();
        }
        

        // Modals 
        private ModalV2 UploadAppModal { get; set; }
        private SidePanelBase AppSettingsPanel { get; set; }
    }
}
