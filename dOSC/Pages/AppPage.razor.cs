using dOSC.Components.Modals;
using dOSCEngine.Services;
using dOSC.Utilities;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.IO;
using static System.Net.Mime.MediaTypeNames;

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

        protected override void OnInitialized()
        {
            Wiresheets = Engine?.GetWireSheets() ?? new();
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
                dOSCWiresheet? wiresheet = new dOSCWiresheet(UploadedFile);
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
            
        }

        private dOSCWiresheetDTO? UploadedFile;
        private void UploadedFileCallback(dOSCWiresheetDTO? Upload)
        {
            if(Upload == null) return;
            UploadedFile = Upload;

        }


        private void DeleteApp()
        {
            DeleteAppModal.Close(); 
            if(Wiresheet == null) return;
            Wiresheet.Desconstruct();
            Wiresheet.Dispose();
            var s = Wiresheet;
            Wiresheet = null;
            Engine.RemoveWiresheet(s);
            Wiresheets = Engine.GetWireSheets();
        }

        private void EditApp()
        {
            
            if (NM != null)
            {
                if(Wiresheet != null)
                {
                    NM.NavigateTo($"apps/editor/{Wiresheet.AppGuid}");
                }
                else
                {
                    NM.NavigateTo($"apps/editor/");
                }
            }
        }

        private async Task DownloadApp()
        {
            if (Wiresheet != null)
            {

                await FileSystem.DownloadWiresheet(JS, Wiresheet);
            }

        }

        

        // Modals 
        private ModalBase NewAppModal { get; set; }
        private ModalBase UploadAppModal { get; set; }
        private ModalBase DeleteAppModal { get; set; }
    }
}
