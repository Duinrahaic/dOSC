using dOSCEngine.Services.User;
using dOSCEngine.Utilities;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static dOSCEngine.Services.Connectors.OSC.OSCService;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace dOSCEngine.Services.Connectors
{
    public abstract class ConnectorBase : IHostedService
    {

        public delegate void ServiceStateChangedHandler(bool state );
        public event ServiceStateChangedHandler? OnServiceStateChanged;

        public virtual string ServiceName => string.Empty;
        public virtual string IconRef => string.Empty;
        public virtual string Description => string.Empty;
        private bool _running = false;
        public bool Running
        {
            get
            {
                return _running;
            }
            set
            {
                _running = value;
                OnServiceStateChanged?.Invoke(_running);
            }
        }
        public bool isRunning() => _running;


        public virtual void LoadSetting()
        {
            throw new NotImplementedException();
        }
        public virtual SettingBase GetSetting()
        {
            throw new NotImplementedException();
        }
        public void UpdateSetting(SettingBase Setting)
        {
            FileSystem.SaveSetting(Setting);
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
