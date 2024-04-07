using dOSCEngine.Services;
using Newtonsoft.Json;

namespace dOSCEngine.Websocket.Commands;

public class dOSCCommandDTO: EventArgs
{
    public string Sender { get; set; }
    public string Target { get; set; } 
    public string Command { get; set; }
    public string DataType { get; set; }
    public string RawData { get; set; } = String.Empty;
    public dOSCData? Data { get; set; }  
    
    public dOSCCommandDTO(string sender, string target, string command, string dataType, string? rawData = null,  dOSCData? data = null)
    {
        Sender = sender;
        Target = target;
        Command = command;
        DataType = dataType;
        Data = data;
        RawData = string.IsNullOrEmpty(rawData) ? String.Empty : rawData;
    }
}