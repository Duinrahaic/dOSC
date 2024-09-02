using Blazor.Diagrams.Core;
using Blazor.Diagrams.Core.Events;
using Blazor.Diagrams.Core.Models.Base;
using BlazorContextMenu;
using dOSC.Component.Modals;
using dOSC.Component.Wiresheet;
using dOSC.Drivers;
using dOSC.Utilities;
using LiveSheet;
using LiveSheet.Parts.Nodes;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.JSInterop;

namespace dOSC.Pages;

public partial class AppEditor : IDisposable
{
    private bool _confirmExit = false;
    private bool _dataHasChanged = false;
    private string _selectedUri = string.Empty;
    

    [Parameter] public string? AppId { get; set; }

    [Inject] private IJSRuntime Js { get; set; }= default!;
    [Inject] public WiresheetService Engine { get; set; } = default!;
    [Inject] public NavigationManager Nm { get; set; }= default!;

    private ModalV2 SaveModal { get; set; } = default!;
    private ModalV2 ExitConfirmationModal { get; set; } = default!;
    private ModalV2 NodeSettingsModal { get; set; } = default!;
    private WiresheetDiagram EditorAppLogic { get; set; } = new();
    private WiresheetDiagram? TempEditorAppLogic = null;

    
    
    private WiresheetDiagram? ReferencedAppLogic { get; set; }
    protected override void OnInitialized()
    {
        EditorAppLogic.RegisterNodes();
        ReferencedAppLogic = Engine.GetAppById(AppId ?? string.Empty);
        if (ReferencedAppLogic != null)
        {
            ReferencedAppLogic.Unload();
            var appdata = ReferencedAppLogic.Data;
            EditorAppLogic.LoadLiveSheetData(appdata);
        }
        EditorAppLogic.Load();
        EditorAppLogic.SelectionChanged += SelectionChanged;
     }


    private void SelectionChanged(SelectableModel obj)
    {
        if (obj is LiveNode node)
        {   
            EditorAppLogic.SendToFront(obj);
        }
    }

    
    protected override void OnAfterRender(bool firstRender)
    {
        if (firstRender)
        {
            EditorAppLogic.Changed += AppChanged;
        }
    }

    bool _isFirstRender = true;
    private void AppChanged()
    {
        if(_isFirstRender)
        {
            _isFirstRender = false;
            return;
        }
        _dataHasChanged = true;
    }
    
    private void Save()
    {
        SaveModal.Open();
        
        if (_confirmExit) Nm!.NavigateTo(_selectedUri ?? "");
    }

    private void SaveCancel()
    {
        _confirmExit = false;
    }
    
    private void SaveApplication()
    {
        EditorAppLogic.UpdateSaveData();
        EditorAppLogic.Unload();

        if (ReferencedAppLogic != null)
        {
            var data = EditorAppLogic.SaveLiveSheetData();
            ReferencedAppLogic.UpdateSaveData(data);
            ReferencedAppLogic.Unload();
            Engine.UpdateApp(ReferencedAppLogic);
            
        }
        else
        {
            Engine.AddApp(EditorAppLogic);
            ReferencedAppLogic = Engine.GetAppById(EditorAppLogic.Guid);
            EditorAppLogic.Load();
        }
        
    }

    private void SaveApp(EditContext context)
    {
        SaveModal.Close();
        if (TempEditorAppLogic == null)
        {
            return;
        }
        EditorAppLogic.Name = TempEditorAppLogic.Name;
        EditorAppLogic.Description = TempEditorAppLogic.Description;
        SaveApplication();        

        TempEditorAppLogic = null;
        _dataHasChanged = false;
    }

  
    
    

    private void RevertApplication()
    {
        _confirmExit = true;
        Nm.NavigateTo("/apps/");
        Nm.NavigateTo($"/editor/{AppId ?? string.Empty}");
    }

    private void OpenAppSettings()
    {
        
    }

    private async Task DownloadApp()
    {
        await Js.DownloadApp(EditorAppLogic);
    }


    private void Exit()
    {
        Nm!.NavigateTo("/apps");
    }

    public void SaveExit()
    {
        ExitConfirmationModal.Close();
        SaveModal.Open();
        TempEditorAppLogic = new()
        {
            Name = EditorAppLogic.Name,
            Description = EditorAppLogic.Description
        };
        _confirmExit = true;
    }

    private void ExitNoSave()
    {
        _confirmExit = true;
        Nm!.NavigateTo(_selectedUri ?? "");
    }

    private void OnLocationChanging(LocationChangingContext context)
    {
        if (!_confirmExit & _dataHasChanged)
        {
            context.PreventNavigation();
            _selectedUri = context.TargetLocation;
            ExitConfirmationModal.Open();
        }
        else
        {
            EditorAppLogic.Unload();
        }
    }

    
   
    public void Dispose()
    {
        EditorAppLogic.Changed -= AppChanged;
        EditorAppLogic.SelectionChanged -= SelectionChanged;
    }
}