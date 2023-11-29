using dOSC.Components.UI.Table;
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
        private string SelectedCategory = "General";

        public void OnSelectedItemChanged(string Category)
        {
            SelectedCategory = Category;
        }

        private void OnConnectorSetting((string, ServiceCommand) e)
        {
            switch (e.Item2)
            {
                case ServiceCommand.Start:
                    //StartService();
                    break;
                case ServiceCommand.Stop:
                    //StopService();
                    break;
                case ServiceCommand.Edit:
                    //OnSelected(UserSettings.Find(x => x.Name == e.Item1));
                    break;
            }
        }
    }
}
