using dOSC.Client.Engine;
using dOSC.Shared.Models.Wiresheet;
using dOSC.Shared.Utilities;
using dOSCEngine.Engine;
using dOSCEngine.Services;
using Microsoft.Extensions.Hosting;
using OSCService = dOSC.Client.Services.Connectors.Hub.OSC.OSCService;

namespace dOSC.Client.Services
{
    public partial class dOSCService : IHostedService
    {
        public readonly string Version = "1";
        private readonly ILogger<OSCService> _logger;
        public readonly ServiceBundle? ServiceBundle;
        private List<AppLogic> _AppMemory = new();

     

        public dOSCService(IServiceProvider services)
        {
            _logger = services.GetService<ILogger<OSCService>>()!;
            _logger.LogInformation("Initialized OSCService");
            ServiceBundle = services.GetService<ServiceBundle>();
            var AppData = GetAppData();
    
            //CompileApps(AppData).ConfigureAwait(true);
        }

        public List<AppLogic> GetApps() => _AppMemory.OrderBy(x=>x.Data.AppName).ToList();
        public AppLogic? GetAppByID(Guid AppId) => _AppMemory.FirstOrDefault(x => x.AppGuid.Equals(AppId));
        private List<dOSCDataDTO> GetAppData() => dOSCFileSystem.LoadApps() ?? new ();

        public async Task AddApp(AppLogic app)
        {
            if(!_AppMemory.Any(x=>x.AppGuid == app.AppGuid))
            {
                _AppMemory.Add(app);
                app.Save();
                //await app.Process();
            }
        }
        public void RemoveApp(AppLogic app) => RemoveApp(app.AppGuid);
        public void RemoveApp(Guid? AppGuid) => RemoveApp(AppGuid.ToString() ?? string.Empty);   
        public void RemoveApp(string AppGuid)
        {
            AppLogic? app = _AppMemory.FirstOrDefault(x => x.AppGuid.ToString() == AppGuid);
        
            if(app != null)
            {
                _AppMemory.Remove(app);
                dOSCFileSystem.RemoveApp(app.AppGuid.ToString());
            }
        }

        public void UpdateApp(AppLogic app)
        {
            AppLogic? appLogic = _AppMemory.FirstOrDefault(x => x.AppGuid == app.AppGuid);

            if(appLogic == null)
            {
                _AppMemory.Add(app);
            }
            else
            {
                _AppMemory.Remove(appLogic);
                _AppMemory.Add(app);
            }
            app.Save();
        }


        private async Task CompileApps(List<dOSCDataDTO> PreCompiledApps)
        {
            foreach (var app in PreCompiledApps)
            {
                await CompileApp(app);
            }
        }

        private async Task CompileApp(dOSCDataDTO PreCompiledApp)
        {
            try
            {
                var CompiledAppData = PreCompiledApp.DeserializeDTO(ServiceBundle!);
                await AddApp(new AppLogic(CompiledAppData, PreCompiledApp.Enabled ? AppState.Enabled : AppState.Disabled, PreCompiledApp.AutomationEnabled ? AutomationState.Paused : AutomationState.Disabled));
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Unable to Compile app {PreCompiledApp.AppGuid}: {ex}");
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
}
