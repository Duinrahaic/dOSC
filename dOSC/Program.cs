using dOSCEngine.Utilities;
using System.Diagnostics;

namespace dOSC
{

    public class Program
    {
        [STAThread]
        public static async Task Main(string[] args)
        {
            bool mutexSuccess = false;
            var globalMutex = new Mutex(true, @"Local\dOSC.exe", out mutexSuccess);

            if (!mutexSuccess)
            {
                Debug.Print("App is already running. Quitting...");
                globalMutex.Close();
                return;
            }
            FileSystem.CreateFolders();
            Webserver.Start(args);

            if (!args.Any(x => x.ToLower().Equals("--headless"))){
                Application.Run(new MainWindow());
            }

            globalMutex.Close();

        }
    }
}