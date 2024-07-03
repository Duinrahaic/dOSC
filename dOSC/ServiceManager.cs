using dOSC.Drivers;
using dOSC.Drivers.Hub;
using dOSC.Drivers.OSC;
using dOSC.Drivers.Pulsoid;
using dOSC.Drivers.VRChat;
using dOSC.Drivers.Websocket;
using dOSC.Drivers.Websocket;

namespace dOSC;

public static class ServiceManager
{
    public static HostApplicationBuilder RegisterServices(this HostApplicationBuilder application)
    {
        application.Services.RegisterHostedService<HubService>(); // Data Service 
        application.Services.AddSingleton<WebSocketHandler>(); // Manages External Service 
        application.Services.RegisterHostedService<WebSocketService>();     
        application.Services.RegisterHostedService<WiresheetService>(); // Manages Applications
        application.Services.RegisterHostedService<PulsoidService>();
        application.Services.RegisterHostedService<OscService>();
        application.Services.RegisterHostedService<VRChatService>();
        application.Services.AddBlazorContextMenu(options =>
            options.ConfigureTemplate(defaultTemplate =>
            {
                defaultTemplate.MenuCssClass = "WireSheet-Context-Menu";
                defaultTemplate.MenuItemCssClass = "WireSheet-Context-Menu-Item";
                defaultTemplate.MenuItemDisabledCssClass = "WireSheet-Context-Menu-Item WireSheet-Context-Menu-Item-Disabled";
                defaultTemplate.MenuListCssClass = "WireSheet-Context-Menu-List";
                defaultTemplate.SeperatorCssClass = "WireSheet-Context-Menu-Separator";
                defaultTemplate.SeperatorHrCssClass = "WireSheet-Context-Menu-Separator-Hr";
                
            })
        );

        return application;
    }
    
    public static void RegisterHostedService<TService>(this IServiceCollection services)
        where TService : class, IHostedService
    {
        // Add the service as a singleton
        services.AddSingleton<TService>();

        // Add the service as a hosted service
        services.AddHostedService<TService>(provider => provider.GetRequiredService<TService>());
    }
}