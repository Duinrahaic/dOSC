using dOSCEngine.Components.Modals;
using dOSCEngine.Engine;
using dOSCEngine.Services;
using dOSCEngine.Utilities;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.JSInterop;

namespace dOSC.Pages
{
    public partial class AppEditor : IDisposable
    {
        [Parameter]
        public Guid? AppId { get; set; }
        [Inject]
        private IJSRuntime? _JS {  get; set; }
        [Inject]
        private ServiceBundle? ServiceBundle { get; set; }
        [Inject]
        public dOSCService? _Engine { get; set; }
        [Inject]
        public NavigationManager? NM { get; set; }
        private ModalV2 SaveModal { get; set; }
        private ModalV2 ExitConfirmationModal { get; set; }
        private ModalV2 NodeSettingsModal { get; set; }
        private AppLogic? EditorAppLogic { get; set; }
        private AppLogic? ReferencedAppLogic { get; set; }  

        protected override async Task OnInitializedAsync()
        {
            if (AppId.HasValue)
            {
                ReferencedAppLogic = _Engine?.GetAppByID(AppId.Value);
                if (ReferencedAppLogic != null)
                {
                    await ReferencedAppLogic.EditApp(true);
                    var DTO = ReferencedAppLogic.GetDTO();
                    EditorAppLogic = new AppLogic(DTO.DeserializeDTO(ServiceBundle), AppState.Enabled, AutomationState.Disabled);
                }
                else
                {
                    EditorAppLogic = new AppLogic(new());
                }
            }
            else
            {
                EditorAppLogic = new AppLogic(new());
            }
            await EditorAppLogic.EditApp(true);
            EditorAppLogic.DiagramDetectedCircularLoop += AppLogic_DiagramDetectedCircularLoop;
            EditorAppLogic.AppDataChanged += AppLogic_AppDataChanged;
        }

        private void AppLogic_AppDataChanged(Guid? AppGuid, bool HasChanged)
        {
            DataHasChanged = HasChanged;
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                if(EditorAppLogic != null)
                {
                    await EditorAppLogic.Load();
                    
                }

            }
        }


        private void AppLogic_DiagramDetectedCircularLoop(string WiresheetGuid)
        {
            try
            {
                _JS.InvokeVoidAsync("GenerateToasterMessage", "Infinite/Circular Link Detected! Removing last link!").ConfigureAwait(false);
            }
            catch
            {

            }
        }
 
   

        
        private void Save()
        {
            if (EditorAppLogic == null) return;
            SaveModal.Open();
            if(ConfirmExit)
            {
                NM!.NavigateTo(SelectedURI ?? "");
            }
        }
        private void SaveCancel()
        {
            ConfirmExit = false;
            SaveModal.Close();
        }

        private async Task SaveApp(EditContext context)
        {
            SaveModal.Close();
            if (EditorAppLogic == null) return;
            if (_Engine == null) return;


            EditorAppLogic.Data.Sync();
            var DTO = EditorAppLogic.GetDTO();
            
            AppLogic? NewAppLogic;
            if (ReferencedAppLogic != null)
            {
                NewAppLogic = new AppLogic(DTO.DeserializeDTO(ServiceBundle), ReferencedAppLogic.AppState, ReferencedAppLogic.AutomationState);

            }
            else
            {
                NewAppLogic = new AppLogic(DTO.DeserializeDTO(ServiceBundle), AppState.Enabled, AutomationState.Disabled);
            }
            await NewAppLogic.EditApp(true);
            _Engine.UpdateApp(NewAppLogic);

            ReferencedAppLogic = NewAppLogic;
            DataHasChanged = false;
        }

        private void Revert()
        {
            ConfirmExit = true;
            NM!.NavigateTo($"/apps/");
            NM!.NavigateTo($"/editor/{EditorAppLogic?.AppGuid}");
        }
        private async Task DownloadApp()
        {
            if (EditorAppLogic != null)
            {
                await FileSystem.DownloadApp(_JS, EditorAppLogic);
            }
        }

      

        public void Dispose()
        {
            if (EditorAppLogic == null) return;
            EditorAppLogic.DiagramDetectedCircularLoop -= AppLogic_DiagramDetectedCircularLoop;
            EditorAppLogic.AppDataChanged -= AppLogic_AppDataChanged;

        }


        private void Exit()
        {
            NM!.NavigateTo($"/apps");
        }

 
        private bool ConfirmExit = false;
        private string? SelectedURI = null;
        private bool DataHasChanged = false;
        public void SaveExit()
        {
            ExitConfirmationModal.Close();
            SaveModal.Open();
            ConfirmExit = true;
        }
        private void ExitNoSave()
        {
            ConfirmExit = true;
            NM!.NavigateTo(SelectedURI ?? "");
        }

        private void OnLocationChanging(LocationChangingContext context)
        {
            if(!ConfirmExit & DataHasChanged == true)
            {
                context.PreventNavigation();
                SelectedURI = context.TargetLocation.ToString();
                ExitConfirmationModal.Open();
            }
            else
            {
                ReferencedAppLogic?.EditApp(false);
                ReferencedAppLogic?.Process();
                EditorAppLogic?.Dispose();
            }
        }
        


    }
}
