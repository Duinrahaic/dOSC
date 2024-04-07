using System.Diagnostics;
using dOSC.Client;
using dOSC.Shared.Utilities;

namespace dOSC;

public static class Program
{
    [STAThread]
    public static async Task Main(string[] args)
    {
        var globalMutex = new Mutex(true, @"Local\dOSC.exe", out var mutexSuccess);
        if (!mutexSuccess)
        {
            Debug.Print("App is already running. Quitting...");
            globalMutex.Close();
            return;
        }

        EncryptionHelper.SetEncryptionKey(Environment.MachineName + "dOSC");
        SetupHub.Start(args);
        SetupClient.Start(args);
        // if (!args.Any(x => x.Equals("--headless", StringComparison.OrdinalIgnoreCase)))
        // {
        //     Application.Run(new MainWindow());
        // }

        while (SetupHub.IsRunning )
        {
            await Task.Delay(1000);
        }
        globalMutex.Close();
        await Task.CompletedTask;
    }
}