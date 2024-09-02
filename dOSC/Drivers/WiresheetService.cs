using dOSC.Component.Wiresheet;
using LiveSheet;

namespace dOSC.Drivers;

public class WiresheetService :  IHostedService
{
    private readonly ILogger<WiresheetService> _logger;
    private Dictionary<string, WiresheetDiagram> Apps { get; set; } = new();

    public WiresheetService(ILogger<WiresheetService> logger)
    {
        _logger = logger;
        _logger.LogInformation("Initialized WiresheetService");
    }

    public int GetAppCount() => Apps.Count;

    public int GetLoadedAppCount() => Apps.Count(x => x.Value.State == LiveSheetState.Loaded);

   

    public List<WiresheetDiagram> GetApps() => Apps.Values.ToList();

    public WiresheetDiagram? GetAppById(string guid) =>
        Apps.TryGetValue(guid, out var app) ? app : null;

    
    
    public void AddApp(WiresheetDiagram app)
    {
        var success = Apps.TryAdd(app.Guid, app);
        
        // TODO: Save to file system
        
        
    }

    public void RemoveApp(WiresheetDiagram app)
    {
        var success = Apps.Remove(app.Guid);
        
        // TODO: Remove to file system
    } 

 
    public void UpdateApp(WiresheetDiagram diagram)
    {
        if (Apps.TryGetValue(diagram.Guid, out var existingApp))
        {
            var newApp = new WiresheetDiagram();
            var data = diagram.SaveLiveSheetData();
            newApp.LoadLiveSheetData(data);
            Apps[diagram.Guid] = newApp;
        }
        else
        {
            AddApp(diagram);
        }
    }
    
    public void ClearApps()
    {
        List<string> keys = Apps.Keys.ToList();
        foreach (var key in keys)
        {
            var app = GetAppById(key);
            if (app == null) continue;
            RemoveApp(app);
        }
    }
    
    public void StopApp(string guid)
    {
        var app = GetAppById(guid);
        if (app?.State == LiveSheetState.Loaded)
        {
            app.Unload();
        }
    }

    public void StartApp(string guid)
    {
        var app = GetAppById(guid);
        if (app?.State == LiveSheetState.Unloaded)
        {
            app.Load();
        }
    }
    
    public Task StartAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}