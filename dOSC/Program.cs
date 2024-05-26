using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using dOSC.Client;
using dOSC.Shared.Utilities;

namespace dOSC;

public class Program
{
    [STAThread]
    public static void Main(string[] args)
    {
        var globalMutex = new Mutex(true, @"Local\dOSC.exe", out var mutexSuccess);
        if (!mutexSuccess)
        {
            Debug.Print("App is already running. Quitting...");
            globalMutex.Close();
            return;
        }

        EncryptionHelper.SetEncryptionKey(Environment.MachineName + "dOSC");
        Debug.Print("Starting Hub...");
        SetupHub.Start(args);
        Debug.Print("Starting Client_Window...");
        //SetupClient.Start(args);
 
        while (SetupHub.IsRunning )
        {
            Task.Delay(1000);
        }
        globalMutex.Close();
    }
}