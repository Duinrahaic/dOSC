using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using dOSC.Utilities;

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
        SetupClient.Start(args);
        globalMutex.Close();
    }
}