using Grpc.Core;

namespace dOSCHub;

public class dOSCHubService: IHostedService
{
    private Server _server;

    public dOSCHubService(Server server)
    {
        _server = server;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _server.Start();
        return Task.CompletedTask;
    }

    public async Task StopAsync(CancellationToken cancellationToken) => await _server.ShutdownAsync();
}