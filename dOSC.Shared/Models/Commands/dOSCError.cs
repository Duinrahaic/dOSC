namespace dOSC.Shared.Models.Commands;

public class dOSCError : dOSCDataPayload
{
    public string Message { get; set; } = string.Empty;
}