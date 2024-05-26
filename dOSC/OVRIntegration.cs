using System;
using System.Runtime.InteropServices;
using System.Threading;
using Valve.VR;

namespace dOSC;

public class OVRIntegration
{
    public static string APPLICATION_KEY = "com.duinrahaic.dOSC";

    private CancellationTokenSource CancelTokenSource = new();

    private CVRSystem? cVR;

    private Thread? pollingThread;

    public bool Initialized { get; private set; }

    public event EventHandler? CloseRequested;

    public bool TryInit(bool startSteamVR = false)
    {
        if (cVR != null)
        {
            Initialized = false;
            return false;
        }

        if (pollingThread != null)
        {
            CancelTokenSource.Cancel();
            pollingThread.Join();
            pollingThread = null;
        }

        var error = EVRInitError.None;
        if (startSteamVR)
            cVR = OpenVR.Init(ref error, EVRApplicationType.VRApplication_Overlay);
        else
            cVR = OpenVR.Init(ref error, EVRApplicationType.VRApplication_Background);

        if (error != EVRInitError.None)
        {
            Initialized = false;
            return false;
        }

        Initialized = true;

        CancelTokenSource = new CancellationTokenSource();
        pollingThread = new Thread(() => PollEvents());
        pollingThread.IsBackground = true;
        pollingThread.Name = "OpenVR Polling";
        pollingThread.Start();

        return true;
    }

    private void PollEvents()
    {
        var evt = new VREvent_t();
        var eventSize = (uint)Marshal.SizeOf(evt);

        while (true)
        {
            if (OpenVR.System.PollNextEvent(ref evt, eventSize))
                if (evt.eventType == (uint)EVREventType.VREvent_Quit)
                {
                    var handler = CloseRequested;
                    handler?.Invoke(this, new EventArgs());
                }

            // we do this instead of thread.sleep
            if (CancelTokenSource.Token.WaitHandle.WaitOne(100)) return; // cancellation was requested
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
        if (cVR == null || !Initialized) return false;

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