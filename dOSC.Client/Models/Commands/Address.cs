namespace dOSC.Client.Models.Commands;

public class Address
{
    public Address()
    {
    }

    public Address(string origin, string destination)
    {
        Origin = origin;
        Destination = destination;
    }

    public string Origin { get; set; } = string.Empty;
    public string Destination { get; set; } = string.Empty;
}