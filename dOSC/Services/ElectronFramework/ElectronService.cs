using ElectronNET.API;
using ElectronNET.API.Entities;

namespace dOSC.Services.ElectronFramework
{
    internal static partial class ElectronFramework
    {
        public static IServiceCollection AddElectronBackend(this IServiceCollection? Services)
        {
            if (Services == null) throw new ArgumentNullException(nameof(Services));
            Services.AddElectron();
            return Services;
        }


    }
}
