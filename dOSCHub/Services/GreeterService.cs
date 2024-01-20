using Grpc.Core;

namespace dOSCHub.Services;

public class GreeterService : Greeter.GreeterBase
{
    public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
    {
        string message;
        if(string.IsNullOrEmpty(request.Name))
            message = "Hello anonymous! dOSC is running!";
        else
            message = "Hello " + request.Name + "! dOSC is running!";

        return Task.FromResult(new HelloReply
        {
            Message = message
        });
    }
}