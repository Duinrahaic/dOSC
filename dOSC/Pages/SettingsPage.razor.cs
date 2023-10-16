using dOSC.Services;
using dOSC.Services.User;
using dOSC.Utilities;

namespace dOSC.Pages
{
    public partial class SettingsPage
    {
        private List<SettingBase> UserSettings = new();
        private SettingBase? Setting = null;
        private SettingType? SettingType = null;
        
        
        protected override void OnInitialized()
        {
            UserSettings = FileSystem.LoadSettings().GetSettings();
        }
        private void OnSelected(SettingBase setting)
        {
            Setting = setting;
        }



    }
}
