using System.Runtime.InteropServices;
using dOSC.Middlewear;
using dOSC.Shared.Models.Websocket;
using dOSC.Component.Modals;
using dOSC.Drivers;
using dOSC.Drivers.Websocket;
using Microsoft.AspNetCore.Components;

namespace dOSC.Component.UI.App;

public partial class AppNav : IDisposable
{
    [Inject] private HubService Server { get; set; }
    public List<NavItem> Apps { get; set; } = new();
    
    private HubModal HubModal { get; set; }
    private bool _alarming = false;

    public void Dispose()
    {
        Server.OnAlarmStateChanged -= AlarmStateUpdated;
    }


    protected override void OnInitialized()
    {
        Server.OnAlarmStateChanged += AlarmStateUpdated;
        _alarming = Server.IsInAlarm;
        Apps.Add(new NavItem("Home", "oi oi-home", "/", NavItemType.Home));
        Apps.Add(new NavItem("Apps", "oi oi-code", "/apps", NavItemType.App));
        Apps.Add(new NavItem("Editor", "icon icon-pencil-ruler", "/editor", NavItemType.App));
        //Apps.Add(new NavItem("Settings", "oi oi-cog", "/settings", NavItemType.Settings));
    }

    private void AlarmStateUpdated(bool inAlarm)
    {
        _alarming = inAlarm;
        InvokeAsync(StateHasChanged);
    }
    
    private void ShowHubModal()
    {
        HubModal.Show();
    }
}