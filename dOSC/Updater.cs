using dOSCEngine.Utilities;
using Squirrel;

namespace dOSC
{
    public static class Update
    {
        public static async Task UpdateApp()
        {
            using (var mgr = new UpdateManager(FileSystem.UpdateFolder))
            {
                if (mgr.IsInstalledApp)
                {
                    await mgr.UpdateApp();
                }
            }
        }
        
        
        
    }
}
