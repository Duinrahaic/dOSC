using Blazor.Diagrams.Core.Anchors;
using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Models.Base;
using dOSC.Client.Engine.Nodes;
using dOSC.Client.Engine.Ports;
using dOSC.Client.Services;
using dOSC.Shared.Models.Wiresheet;
using dOSC.Shared.Utilities;
using Newtonsoft.Json;

namespace dOSC.Client.Engine;

public class AppLogic : IDisposable
{
    public delegate void OnAppDataChanged(Guid? AppGuid, bool HasChanged);

    // Events
    public delegate void OnAppStateChanged(Guid? AppGuid, AppState AppState, AutomationState AutomationState);

    public delegate void OnAppStatusChanged(Guid? AppGuid, AppStatus AppStatus);

    public delegate void OnDiagramDetectedCircularLoop(string WiresheetGuid);

    private bool AutomationEnabled;


    // App State
    [JsonIgnore] private bool Built;


    // Constructor 
    public AppLogic(dOSCData wiresheet, AppState appState = AppState.Disabled,
        AutomationState autoState = AutomationState.Disabled)
    {
        Data = wiresheet;
        if (Data != null)
        {
            AppState = appState;
            AutomationState = autoState;
        }
    }

    // Variables
    public dOSCData Data { get; set; } = new(); // The app design.
    private dOSCService? engine { get; set; }
    public Guid? AppGuid => Data?.AppGuid;
    public bool InEditMode { get; private set; }

    public AppState AppState { get; private set; }

    public AutomationState AutomationState { get; private set; } = AutomationState.Disabled;

    public AppStatus AppStatus { get; private set; } = AppStatus.Disabled;

    public void Dispose()
    {
        Unload().ConfigureAwait(false);
    }

    public event OnAppStateChanged? AppStateChanged;
    public event OnAppStatusChanged? AppStatusChanged;
    public event OnAppDataChanged? AppDataChanged;
    public event OnDiagramDetectedCircularLoop? DiagramDetectedCircularLoop;

    public bool IsRunning()
    {
        return Built;
    }

    public bool IsEnabled()
    {
        return AppState == AppState.Enabled;
    }

    public bool IsAutomated()
    {
        return AutomationEnabled;
    }

    public void ReplaceData(dOSCData data)
    {
        if (Data != null)
        {
            Data = data;
            Save();
        }
    }


    public async Task EditApp(bool Edit)
    {
        if (Edit)
        {
            await Unload();
            InEditMode = true;
        }
        else
        {
            await Unload();
            await Load();
            InEditMode = false;
        }

        await Process();
    }

    public async Task EnableAutomation()
    {
        AutomationEnabled = true;
        AutomationState = AutomationState.Paused;
        await Process();
        AppStateChanged?.Invoke(AppGuid, AppState, AutomationState);
    }

    public async Task DisableAutomation()
    {
        AutomationEnabled = false;
        AutomationState = AutomationState.Disabled;
        await Process();
        AppStateChanged?.Invoke(AppGuid, AppState, AutomationState);
    }

    public async Task EnableApp()
    {
        AppState = AppState.Enabled;
        await Process();
        AppStateChanged?.Invoke(AppGuid, AppState, AutomationState);
    }

    public async Task DisableApp()
    {
        AppState = AppState.Disabled;
        await Process();
        AppStateChanged?.Invoke(AppGuid, AppState, AutomationState);
    }

    public async Task Process()
    {
        if (InEditMode)
        {
            AppStatus = AppStatus.Editing;
            AppStatusChanged?.Invoke(AppGuid, AppStatus);
            return;
        }

        if (AppState == AppState.Enabled)
        {
            if (AutomationState == AutomationState.Running)
            {
                await Load();
                AppStatus = AppStatus.Running;
            }
            else if (AutomationState == AutomationState.Paused)
            {
                await Unload();
                AppStatus = AppStatus.AutoPaused;
            }
            else
            {
                await Load();
                AppStatus = AppStatus.AutoRunning;
            }
        }
        else if (AppState == AppState.Disabled)
        {
            await Unload();
            AppStatus = AppStatus.Disabled;
        }
        else
        {
            AppStatus = AppStatus.Unknown;
        }

        AppStatusChanged?.Invoke(AppGuid, AppStatus);
    }

    public async Task Load()
    {
        if (!Built)
        {
            Data.Diagram.Nodes.Added += OnNodeAdded;
            Data.Diagram.Nodes.Removed += OnNodeRemoved;
            Data.Diagram.Links.Added += OnLinkAdded;
            Data.Diagram.Links.Removed += OnLinkRemoved;
            foreach (var node in Data.GetAllNodes()) Data.Diagram.Nodes.Add(node);
            foreach (var l in Data.GetAllLinks())
            {
                var BlockSource =
                    Data.Diagram.Nodes.FirstOrDefault(x => (x as BaseNode)?.Guid == l.SourcePort.ParentGuid);
                var BlockTarget =
                    Data.Diagram.Nodes.FirstOrDefault(x => (x as BaseNode)?.Guid == l.TargetPort.ParentGuid);

                if (BlockSource != null && BlockTarget != null)
                {
                    var sourcePort = BlockSource.Ports.FirstOrDefault(x => (x as BasePort)?.Guid == l.SourcePort.Guid);
                    var targetPort = BlockTarget.Ports.FirstOrDefault(x => (x as BasePort)?.Guid == l.TargetPort.Guid);
                    if (sourcePort != null && targetPort != null)
                    {
                        Data.Diagram.Links.Add(new LinkModel(sourcePort, targetPort));
                        var sp = sourcePort as BasePort;
                        sp?.PortUpdated(sp);

                        var tp = targetPort as BasePort;
                        tp?.PortUpdated(tp);
                    }
                }
            }


            Built = true;

            Data.Diagram.Refresh();

            foreach (var n in Data.Diagram.Nodes)
                if (n as BaseNode is { } node)
                    foreach (var port in node.Ports)
                        port.Refresh();


            Data.Diagram.ContainerChanged += Diagram_ContainerChanged;
        }

        await Task.CompletedTask;
    }

    public async Task Unload()
    {
        Data.Diagram.Nodes.Added -= OnNodeAdded;
        Data.Diagram.Nodes.Removed -= OnNodeRemoved;
        Data.Diagram.Links.Added -= OnLinkAdded;
        Data.Diagram.Links.Removed -= OnLinkRemoved;
        Data.Diagram.ContainerChanged -= Diagram_ContainerChanged;
        Data.Diagram.Nodes.Clear();
        Data.Diagram.Links.Clear();
        Data.Diagram.Refresh();
        await Task.CompletedTask;
    }

    public void SetAppIcon(string NewBase64Image)
    {
        Data.AppIcon = NewBase64Image;
        Save();
    }

    public void ResetAppIcon()
    {
        SetAppIcon(AppDefaults.GetDefaultAppImage());
    }

    //Event Call Backs
    private void Diagram_ContainerChanged()
    {
        try
        {
            Data.Diagram.ZoomToFit(200);
        }
        catch (Exception ex)
        {
        }

        Data.Diagram.ContainerChanged -= Diagram_ContainerChanged;
    }

    private void OnNodeAdded(NodeModel node)
    {
        var n = node as BaseNode;
        n!.ValueChanged += OnValueChanged;
        AppDataChanged?.Invoke(AppGuid, true);
    }

    private void OnNodeRemoved(NodeModel node)
    {
        var n = node as BaseNode;
        n!.ValueChanged -= OnValueChanged;
        AppDataChanged?.Invoke(AppGuid, true);
    }

    private void OnValueChanged(BaseNode op)
    {
        if (Data.Diagram != null)
        {
            foreach (var link in op.Ports.Where(x => !(x as BasePort).Input).SelectMany(x => x.Links))
            {
                // Future Duin: Prevent links with errors from executing

                var sp = (link.Source as SinglePortAnchor)!;
                var tp = (link.Target as SinglePortAnchor)!;
                if (sp != null && tp != null)
                {
                    var InputPort = (sp.Port as BasePort)!.Input ? sp : tp;
                    if (InputPort != null) (InputPort.Port.Parent as BaseNode)!.CalculateValue();

                    //var otherNode = sp.Port.Parent == op ? tp.Port.Parent : sp.Port.Parent;
                    //otherNode.Refresh();
                }
            }

            op.Refresh();
        }
    }

    private void OnLinkAdded(BaseLinkModel link)
    {
        link.TargetChanged += OnLinkTargetChanged;
        link.Changed += OnLinkChanged;
        var sp = (link.Source as SinglePortAnchor)!;
        var tp = (link.Target as SinglePortAnchor)!;

        if (sp != null && tp != null)
        {
            var IsCircular = new GraphUtilities().CheckForCircularLinks(Data.Diagram);
            if (!IsCircular)
            {
                var InputPort = (sp.Port as BasePort)!.Input ? sp : tp;
                if (InputPort != null) (InputPort.Port.Parent as BaseNode)!.CalculateValue();
            }
        }


        (sp?.Port as BasePort)?.UpdateLinkCount();
        (tp?.Port as BasePort)?.UpdateLinkCount();
        AppDataChanged?.Invoke(AppGuid, true);
    }

    private void OnLinkChanged(Model obj)
    {
        var link = obj as BaseLinkModel;
    }


    private void OnLinkRemoved(BaseLinkModel link)
    {
        try
        {
            Data.Diagram.Links.Remove(link);
        }
        catch (Exception ex)
        {
        }

        link.TargetChanged -= OnLinkTargetChanged;
        link.Changed += OnLinkChanged;
        (link.Source.Model as PortModel)!.Parent.Refresh();
        var sp = (link.Source as SinglePortAnchor)!;
        var tp = (link.Target as SinglePortAnchor)!;
        (sp?.Port as BasePort)?.UpdateLinkCount();
        (tp?.Port as BasePort)?.UpdateLinkCount();
        Data.Diagram.Nodes.ToList().Where(x => x != null).ToList().ForEach(x => x.Refresh());
        AppDataChanged?.Invoke(AppGuid, true);
    }

    private void OnLinkTargetChanged(BaseLinkModel link, Anchor? oldTarget, Anchor? newTarget)
    {
        var sp = (link.Source as SinglePortAnchor)!;
        var tp = (link.Target as SinglePortAnchor)!;

        if (sp != null && tp != null)
        {
            var IsCircular = new GraphUtilities().CheckForCircularLinks(Data.Diagram);
            if (IsCircular) Data.Diagram.Links.Remove(link);
        }

        if (oldTarget.Model == null && newTarget.Model != null) // First attach
            (newTarget.Model as BasePort)!.Parent.Refresh();

        AppDataChanged?.Invoke(AppGuid, true);
    }


    public void Save()
    {
        try
        {
            dOSCFileSystem.SaveApp(GetDTO());
        }
        catch
        {
        }
    }

    public dOSCDataDTO? GetDTO()
    {
        return Data.GetDTO(IsEnabled(), IsAutomated());
    }
}