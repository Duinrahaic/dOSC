using dOSC.Component.Wiresheet;
using LiveSheet;

namespace dOSC.Drivers;

public class WiresheetService : IHostedService
{
    private readonly ILogger<WiresheetService> _logger;
    public Dictionary<string,WiresheetDiagram> Apps { get; private set; } = new();
    
    public WiresheetService(ILogger<WiresheetService> logger)
    {
        _logger = logger;
        _logger.LogInformation("Initialized WiresheetService Service");
    }

    public void AddApp(WiresheetDiagram app)
    {
        Apps.Add(app.Guid,app);
    }
    
    public void RemoveApp(WiresheetDiagram app)
    {
        Apps.Remove(app.Guid);
    }
    
    public List<WiresheetDiagram> GetApps()
    {
        return Apps.Select(x=> x.Value ).ToList();
    }
    public WiresheetDiagram? GetAppById(string guid)
    {
        return Apps.FirstOrDefault(x => x.Key == guid).Value;
    }

    public void UpdateApp(WiresheetDiagram diagram)
    {
        var app = Apps.FirstOrDefault(x => x.Key == diagram.Guid).Value;
        if(app != null)
        {
            if (app.State == LiveSheetState.Loaded)
            {
                app.Unload();
                app = diagram;
                app.Load();
            }
            else if (app.State == LiveSheetState.Unloaded)
            {
                var newApp = new WiresheetDiagram();
                newApp.LoadLiveSheetData(diagram.SerializeLiveSheet());
                app = newApp;
            }
        }
        else
        {
            var newApp = new WiresheetDiagram();
            newApp.LoadLiveSheetData(diagram.SerializeLiveSheet());
            Apps.Add(diagram.Guid, newApp);
        }
    }
    
    public Task StartAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}