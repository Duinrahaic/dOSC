using dOSCEngine.Services;
using dOSCEngine.Services.Connectors.Activity.Pulsoid;
using dOSCEngine.Services.User;
using dOSCEngine.Utilities;
using Microsoft.AspNetCore.Components;
using System;

namespace dOSC.Pages
{
    public partial class SettingsPage
    {
        private List<SettingBase> UserSettings = new();
        private SettingBase? Setting = null;
        private SettingType? SettingType = null;

        [Inject]
        public PulsoidService? _Pulsoid { get;set; } 
        
        protected override void OnInitialized()
        {
            UserSettings = FileSystem.LoadSettings().GetSettings();
        }
        private void OnSelected(SettingBase setting)
        {
            Setting = setting;
        }

        private async Task SettingChanged(SettingBase setting)
        {
            if (Setting == null)
                return;
            var settings = FileSystem.LoadSettings() ?? new();
            switch (setting.SettingType)
            {
                case dOSCEngine.Services.User.SettingType.Pulsoid:
                    settings.Pulsoid = (PulsoidSetting)setting;
                    Setting = (PulsoidSetting)setting;
                    if (_Pulsoid == null) return;
                    _Pulsoid.UpdateSetting((PulsoidSetting)Setting);
                    break;
            }
            FileSystem.SaveSettings(settings);


        }
        private void StartService()
        {
            if (Setting == null)
                return;
            switch (Setting.SettingType)
            {
                case dOSCEngine.Services.User.SettingType.Pulsoid:
                    if (_Pulsoid == null) return;
                    _Pulsoid.Start();
                    break;
            }

            
        }
        private void StopService()
        {
            if (Setting == null)
                return;
            switch (Setting.SettingType)
            {
                case dOSCEngine.Services.User.SettingType.Pulsoid:
                    if (_Pulsoid == null) return;
                    _Pulsoid.Stop();
                    break;
            }
        }

    }
}
