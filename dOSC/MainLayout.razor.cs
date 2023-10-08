using ElectronNET.API;

namespace dOSC
{
    public partial class MainLayout
    {

        public void Minimize()
        {
            try
            {
                var window = Electron.WindowManager.BrowserWindows.First();
                window.Minimize();
            }
            catch
            {

            }
        }
        public void Maximize() {
            try
            {
                var window = Electron.WindowManager.BrowserWindows.First();
                window.Maximize();
            }
            catch
            {

            }
        }

        public void Close()
        {
            try
            {
                var window = Electron.WindowManager.BrowserWindows.First();
                window.Close();
            }
            catch
            {

            }
            
        }
    }
}
