using Grpc.Core;


namespace dOSCHub.Services;

public class DataService: dOSCHub.Data.DataBase
{
    public override Task<SuccessMessage> WriteData(DataWriteRequest request, ServerCallContext context)
    {
        string message;
        if(string.IsNullOrEmpty(request.Name))
            message = "Hello anonymous! dOSC is running!";
        else
            message = "Hello " + request.Name + "! dOSC is running!";

        return Task.FromResult(new SuccessMessage
        {
            Message = message,
            Success = true,
            Timestamp = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(DateTime.UtcNow)
        });
    }
}