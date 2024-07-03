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
    private WiresheetDiagram? ReferencedAppLogic { get; set; }
    protected override void OnInitialized()
    {
        EditorAppLogic.RegisterNodes();
        ReferencedAppLogic = Engine.GetAppById(AppId ?? string.Empty);
        if (ReferencedAppLogic != null)
        {
            ReferencedAppLogic.Unload();
            var appdata = ReferencedAppLogic.SerializeLiveSheet();
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
        SaveModal.Close();
    }

    private void SaveApp(EditContext context)
    {
        SaveModal.Close();
        EditorAppLogic.UpdateSaveData();
        if (ReferencedAppLogic != null)
        {
            ReferencedAppLogic.LoadLiveSheetData(EditorAppLogic.SerializeLiveSheet());
        }
        else
        {
            Engine.AddApp(EditorAppLogic);
        }
        _dataHasChanged = false;
    }

    private void Revert()
    {
        _confirmExit = true;
        Nm!.NavigateTo("/apps/");
        Nm!.NavigateTo($"/editor/{EditorAppLogic?.Guid}");
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