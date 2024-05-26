using dOSC.Component.Wiresheet;
using LiveSheet;

namespace dOSC.Drivers;

public class WiresheetService : IHostedService
{
    
    public List<WiresheetDiagram> Apps { get; private set; } = new();
    
    public WiresheetService()
    {
            
    }

    public void AddApp(WiresheetDiagram app)
    {
        Apps.Add(app);
    }
    
    public List<WiresheetDiagram> GetApps()
    {
        return Apps;
    }
    public WiresheetDiagram? GetAppId(string guid)
    {
        return Apps.FirstOrDefault(x => x.Guid == guid);
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