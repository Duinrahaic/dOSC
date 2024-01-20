using dOSCEngine.Components.UI.Table;

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
