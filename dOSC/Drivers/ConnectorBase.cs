using System;
using System.Threading;
using System.Threading.Tasks;
using dOSC.Attributes;
using dOSC.Shared.Models.Settings;
using dOSC.Utilities;
using dOSC.Utilities;
using Microsoft.Extensions.Hosting;

namespace dOSC.Drivers;

public abstract class ConnectorBase : IHostedService
{
    public delegate void ServiceStateChangedHandler(bool state);

    private bool _running;

    public virtual string ServiceName => string.Empty;
    public virtual string IconRef => string.Empty;
    public virtual string Description => string.Empty;

    internal readonly HubService HubService;
    
    public ConnectorBase(IServiceProvider services)
    {
        HubService = services.GetService<HubService>();

        var endpoints = EndpointHelper.GetEndpoints(this);
        foreach (var endpoint in endpoints)
        {
            HubService.RegisterEndpoint(endpoint);
        }
    }
    
    
    public bool Running
    {
        get => _running;
        set
        {
            _running = value;
            OnServiceStateChanged?.Invoke(_running);
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

    public event ServiceStateChangedHandler? OnServiceStateChanged;

    public bool IsRunning()
    {
        return _running;
    }


    public virtual void LoadSetting()
    {
        throw new NotImplementedException();
    }

    public virtual SettingBase GetSetting()
    {
        throw new NotImplementedException();
    }

    public void UpdateSetting(SettingBase setting)
    {
        AppFileSystem.SaveSetting(setting);
    }

    public virtual void StartService()
    {
        throw new NotImplementedException();
    }

    public virtual void StopService()
    {
        throw new NotImplementedException();
    }

    public virtual void RestartService()
    {
        StopService();
        StartService();
    }
}