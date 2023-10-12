using ElectronNET.API;

namespace dOSC.Services.ElectronFramework
{
	internal static partial class ElectronFramework
	{
		public static void OpenExternal(string url)
		{
			Electron.Shell.OpenExternalAsync(url).Wait();
		}

	}
}
