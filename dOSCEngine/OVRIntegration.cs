using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 using Valve.VR;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace dOSCEngine
{
    public class OVRIntegration
    {
        public static string APPLICATION_KEY = "com.duinrahaic.dOSC";

        private CVRSystem? cVR;

        private bool initialized = false;
        public bool Initialized
        {
            get { return initialized; }
        }

        public event EventHandler? CloseRequested;

        private CancellationTokenSource CancelTokenSource = new CancellationTokenSource();
        private Thread? pollingThread = null;

        public bool TryInit(bool startSteamVR = false)
        {
            if (cVR != null)
            {
                this.initialized = false;
                return false;
            }

            if (pollingThread != null)
            {
                CancelTokenSource.Cancel();
                pollingThread.Join();
                pollingThread = null;
            }

            EVRInitError error = EVRInitError.None;
            if (startSteamVR)
            {
                cVR = OpenVR.Init(ref error, EVRApplicationType.VRApplication_Overlay);
            } else
            {
                cVR = OpenVR.Init(ref error, EVRApplicationType.VRApplication_Background);
            }

            if (error != EVRInitError.None)
            {
                this.initialized = false;
                return false;
            }
            else
            {
                this.initialized = true;

                CancelTokenSource = new CancellationTokenSource();
                pollingThread = new Thread(() => PollEvents());
                pollingThread.IsBackground = true;
                pollingThread.Name = "OpenVR Polling";
                pollingThread.Start();

                return true;
            }
        }

        private void PollEvents()
        {
            VREvent_t evt = new VREvent_t();
            uint eventSize = (uint)Marshal.SizeOf(evt);

            while (true)
            {
                if (OpenVR.System.PollNextEvent(ref evt, eventSize))
                {
                    if (evt.eventType == (uint)EVREventType.VREvent_Quit)
                    {
                        var handler = CloseRequested;
                        handler?.Invoke(this, new EventArgs());
                    }
                }

                // we do this instead of thread.sleep
                if (CancelTokenSource.Token.WaitHandle.WaitOne(100))
                {
                    return; // cancellation was requested
                }
            }
        }

        public void Shutdown()
        {
            if (cVR != null)
            {
                OpenVR.Shutdown();
                cVR = null;
            }
        }

        public bool IsInstalled()
        {
            if (cVR == null || !initialized)
            {
                return false;
            }

            return OpenVR.Applications.IsApplicationInstalled(APPLICATION_KEY);
        }

        //public void InstallManifest()
        //{
        //    if (cVR == null || !initialized)
        //    {
        //        return;
        //    }

        //    var executablePath = Application.ExecutablePath;
        //    var executableDir = Path.GetDirectoryName(executablePath);

        //    EVRApplicationError error = OpenVR.Applications.AddApplicationManifest(Path.Join(executableDir, "manifest.vrmanifest"), false);

        //    if (error != EVRApplicationError.None)
        //    {
        //        MessageBox.Show("Error while registering with SteamVR: " + error.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    } else
        //    {
        //        MessageBox.Show("Successfully registered with SteamVR", "Success");
        //    }
        //}

        //public void UninstallManifest()
        //{
        //    if (cVR == null || !initialized)
        //    {
        //        return;
        //    }

        //    StringBuilder sb = new StringBuilder("", 512);
        //    EVRApplicationError error = EVRApplicationError.None;

        //    OpenVR.Applications.GetApplicationPropertyString(APPLICATION_KEY, EVRApplicationProperty.WorkingDirectory_String, sb, 512, ref error);

        //    var manifestPath = Path.Join(sb.ToString(), "manifest.vrmanifest");
        //    error = OpenVR.Applications.RemoveApplicationManifest(manifestPath);

        //    if (error != EVRApplicationError.None)
        //    {
        //        MessageBox.Show("Error while unregistering from SteamVR: " + error.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //    else
        //    {
        //        MessageBox.Show("Successfully unregistered from SteamVR", "Success");
        //    }
        //}
    }
}
