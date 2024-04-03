
using dOSC.Services;
using dOSCEngine.Utilities;
using dOSCEngine;
using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Server.Kestrel.Core;


namespace dOSC
{
    public static class SetupHub
    {
        public static bool IsRunning { get; private set; }
        private static WebApplication? _app;
		
        public static List<string> GetUrls() => IsRunning ? _app?.Urls.ToList() ??new() : new();
  
        public static async void Start(string[] args)
        {
	        var builder = WebApplication.CreateBuilder(new WebApplicationOptions() { ContentRootPath = System.IO.Directory.GetCurrentDirectory(), ApplicationName = nameof(dOSCClient) });
	        
            FileSystem.CreateFolders();

            builder.WebHost.UseStaticWebAssets();
            
#if DEBUG
            int port = 5232;
#else 
            var settings = FileSystem.LoadSettings();
	        int port = settings?.dOSC.GetHubServerPort() ?? 5232;
#endif
            string url = $@"http://localhost:{port}";

            builder.WebHost.UseKestrel();
            builder.WebHost.ConfigureKestrel(options =>
            {
	            options.ListenLocalhost(port, o => o.Protocols =
		            HttpProtocols.Http2);
            });
            
            
   
            builder.Services.AddGrpc();
            
            builder.AddHubServices();
            builder.AddLogging();

            _app = builder.Build();
            // Setup the HTTP request pipeline.
            if (!_app.Environment.IsDevelopment())
            {
                _app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                _app.UseHsts();
            }

            _app.UseGrpcWeb(new GrpcWebOptions { DefaultEnabled = true });
 
            _app.UseRouting();
 
            _app.MapGrpcService<GreeterService>().EnableGrpcWeb();
            _app.MapGet("/", () => "This gRPC service is gRPC-Web enabled and is callable from browser apps using the gRPC-Web protocol");
            
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
