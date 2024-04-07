using dOSC.Shared.Utilities;

namespace dOSC.Client;

public class Program
{
    [STAThread]
    public static async Task Main(string[] args)
    {
        SetupClient.Start(args);
        await Task.CompletedTask;
    }
}