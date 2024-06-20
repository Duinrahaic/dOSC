using System;
using System.Threading;
using System.Threading.Tasks;
using dOSC.Attributes;
using dOSC.Drivers.Settings;
using dOSC.Utilities;
using dOSC.Utilities;
using Microsoft.Extensions.Hosting;
using dOSC.Drivers.Hub;

namespace dOSC.Drivers;

public abstract class ConnectorBase : IHostedService
{
    public delegate void StateChanged(bool running);
    public event StateChanged? OnStateChanged;
    public virtual SettingBase Configuration { get; set; } = new();
    public virtual string Name => string.Empty;
    public virtual string IconRef => string.Empty;
    public virtual string Description => string.Empty;
    internal readonly HubService HubService;

    public ConnectorBase(IServiceProvider services)
    {
        HubService = services.GetService<HubService>();
        _enabled = Configuration.Enabled;
        var endpoints = EndpointHelper.GetEndpoints(this);
        foreach (var endpoint in endpoints)
        {
            HubService.RegisterEndpoint(endpoint);
        }
    }
    
    internal bool _enabled { get; set; }= true;
    public virtual bool Enabled
    {
        get => _enabled;
        set
        {
            if (_enabled != value)
            {
                _enabled = value;
                Configuration.Enabled = value;
                SaveConfiguration(Configuration);
            }
        }
    }
    
    internal void InvokeStateChanged(bool running) => OnStateChanged?.Invoke(running);
    
    public virtual Task StartAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    public virtual Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    private bool _running = false;
    public bool Running
    {
        get => _running;
        set
        {
            if (_running != value)
            {
                _running = value;
                InvokeStateChanged(value);
            }
        }
    }
    public virtual void SaveConfiguration(SettingBase configuration)
    {
        AppFileSystem.SaveSetting(configuration);
        Configuration = configuration;
    }
    

    public virtual void StartService() => throw new NotImplementedException();

    public virtual void StopService() => throw new NotImplementedException();

    public virtual void RestartService()
    {
        StopService();
        StartService();
    }

    protected void UpdateEndpointValue(string name, object value)
    {
        var endpoint = EndpointHelper.GetDefaultEndpoint(this, name);
        if (endpoint != null)
        {
            var ev = endpoint.ToDataEndpointValue();
            if (value is Boolean bv)
            {
                ev.UpdateValue(bv);
                HubService.UpdateEndpointValue(ev);
            }
            else if (value is String sv)
            {
                ev.UpdateValue(sv);
                HubService.UpdateEndpointValue(ev);
            }
            else if (value is decimal dv)
            {
                ev.UpdateValue(dv);
                HubService.UpdateEndpointValue(ev);
            }
        }
    }
    
    
    
}