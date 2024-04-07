/*
using dOSCEngine;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using dOSC.Client;

namespace dOSC
{
    internal class dOSCWebInterfaceContext: ApplicationContext
    {
        private NotifyIcon trayIcon;
        private OVRIntegration ovr;


        public dOSCWebInterfaceContext()
        {
            var iconStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("dOSCApp.icon.ico");

            if (iconStream == null)
            {
                throw new Exception("Trayicon could not be loaded");
            }

            var icon = new Icon(iconStream);

            trayIcon = new NotifyIcon()
            {
                Icon = icon,
                ContextMenuStrip = new ContextMenuStrip(),
                Visible = true,
                Text = "dOSC",
            };


            ovr = new OVRIntegration();
            ovr.TryInit();

            ovr.CloseRequested += CloseRequestedHandler;

            setupTrayMenu();
        }

        private void setupTrayMenu()
        {
            trayIcon.ContextMenuStrip.Items.Clear();

            if (ovr.Initialized)
            {
                if (ovr.IsInstalled())
                {
                    trayIcon.ContextMenuStrip.Items.Add(new ToolStripMenuItem("Unregister from SteamVR", null, UnregisterSteamVR));
                }
                else
                {
                    trayIcon.ContextMenuStrip.Items.Add(new ToolStripMenuItem("Register with SteamVR", null, RegisterSteamVR));
                }
            }
            else
            {
                trayIcon.ContextMenuStrip.Items.Add(new ToolStripMenuItem("Listen/Attach to SteamVR", null, AttachSteamVR));
            }

            trayIcon.ContextMenuStrip.Items.Add(new ToolStripSeparator());

            trayIcon.ContextMenuStrip.Items.Add(new ToolStripMenuItem("Exit", null, Exit));
        }

        private void CloseRequestedHandler(object? sender, EventArgs args)
        {
            Exit(null, new EventArgs());
        }

        private void AttachSteamVR(object? sender, EventArgs e)
        {
            ovr.TryInit(true);
            setupTrayMenu();
        }

        private void RegisterSteamVR(object? sender, EventArgs e)
        {
            //ovr.InstallManifest();
            setupTrayMenu();
        }

        private void UnregisterSteamVR(object? sender, EventArgs e)
        {
            //ovr.UninstallManifest();
            setupTrayMenu();
        }


        private void Exit(object? sender, EventArgs e)
        {
            SetupClient.Stop();
            trayIcon.Visible = false;
            Application.Exit();
        }
    }
}
*/

