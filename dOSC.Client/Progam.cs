using dOSC.Shared.Utilities;

namespace dOSC.Client;

public class Program
{
    [STAThread]
    public static void Main(string[] args)
    {
        SetupClient.Start(args);
    }
}