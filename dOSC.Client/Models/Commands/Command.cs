using System;

namespace dOSC.Client.Models.Commands;

public class Command : EventArgs
{
    public Command()
    {
    }

    public Command(string origin, string destination, string action, string type, Data data)
    {
        Address = new Address(origin, destination);
        Data = data;
    }

    public Address Address { get; set; } = new();
    public CommandType Type { get; set; } = CommandType.Log;
    public Data? Data { get; set; }
}