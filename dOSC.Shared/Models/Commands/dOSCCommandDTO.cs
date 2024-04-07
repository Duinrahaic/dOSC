using System;

namespace dOSC.Shared.Models.Commands;

public class dOSCCommandDTO : EventArgs
{
    public dOSCCommandDTO(string sender, string target, string command, string dataType, string? rawData = null,
        dOSCDataPayload? data = null)
    {
        Sender = sender;
        Target = target;
        Command = command;
        DataType = dataType;
        Data = data;
        RawData = string.IsNullOrEmpty(rawData) ? string.Empty : rawData;
    }

    public string Sender { get; set; }
    public string Target { get; set; }
    public string Command { get; set; }
    public string DataType { get; set; }
    public string RawData { get; set; } = string.Empty;
    public dOSCDataPayload? Data { get; set; }
}