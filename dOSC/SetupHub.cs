using System;
using System.Collections.Generic;
using System.Linq;
using dOSCEngine.Utilities;
using dOSCEngine;
using dOSCEngine.Websocket;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Hosting;


namespace dOSC.Hub
{
    public static class SetupHub
    {
        public static bool IsRunning { get; private set; }
        private static WebApplication? _app;
		
        public static List<string> GetUrls() => IsRunning ? _app?.Urls.ToList() ??new() : new();
  
        public static async void Start(string[] args, LogSink? sink = null, string key = "")
		{

	        var builder = WebApplication.CreateBuilder(new WebApplicationOptions() { ContentRootPath = System.IO.Directory.GetCurrentDirectory(), ApplicationName = nameof(dOSCEngine) });
	        key = string.IsNullOrEmpty(key) ? SecureKeyGenerator.GenerateSecureKey() : key;
            dOSCFileSystem.CreateFolders();

            builder.WebHost.UseStaticWebAssets();
#if DEBUG
            int port = 5232;
#else 
            var settings = dOSCFileSystem.LoadSettings();
	        int port = settings?.dOSC.GetHubServerPort() ?? 5232;
#endif
            string url = $@"http://localhost:{port}";

            builder.WebHost.UseKestrel();
            builder.WebHost.ConfigureKestrel(options =>
            {
	            options.ListenLocalhost(port, o => o.Protocols =
		            HttpProtocols.Http1);
            });
            
            builder.AddHubServices();
            
            builder.AddLogging(sink);

            
            _app = builder.Build();
            
            // Setup the HTTP request pipeline.
            if (!_app.Environment.IsDevelopment())
            {
                _app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                _app.UseHsts();
            }

            _app.UseWebSockets();
            _app.UseMiddleware<WebSocketMiddleware>( sink, key);
            IsRunning = true;

	        _app.Urls.Add(url);
            if (!args.Any(x => x.Equals("--headless", StringComparison.CurrentCultureIgnoreCase )))
			{
				await _app.RunAsync();
			}
			else
			{
				_app.Run();
			}
		}

	
 


		public static void Stop()
        {
            IsRunning = false;
            _app?.StopAsync();
        }
    }
}
