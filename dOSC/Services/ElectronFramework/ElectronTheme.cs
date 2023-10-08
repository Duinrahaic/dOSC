using ElectronNET.API;

namespace dOSC.Services.ElectronFramework
{
    internal static partial class ElectronFramework
    {
        public static WebApplicationBuilder? ConfigureElectronTheme(this WebApplicationBuilder? builder)
        {
            // Contact @Lightbulb4 on twitter for details
            Electron.NativeTheme.ShouldUseDarkColorsAsync();
            return builder;
        }
    }
}
