using System;

namespace dOSC.Client.Models.Commands;

public class DataEndpoint : Data
{
    public string Owner { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Alias { get; set; } = string.Empty;
    public DataType Type { get; set; } = DataType.Text;
    public string DefaultValue { get; set; } = string.Empty;
    public Permissions Permissions { get; set; } = Permissions.ReadWrite;

    public DataLabels Labels { get; set; } = new();
    public Constraints Constraints { get; set; } = new();

    public override string ToString()
    {
        return
            $"Owner: {Owner}, Name: {Name}, Alias: {Alias}, Type: {Type}, Value: {DefaultValue}, Permissions: {Permissions}";
    }

    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType()) return false;

        if (obj is DataEndpoint ep) return Owner == ep.Owner && Name == ep.Name;

        return false;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Owner, Name);
    }


    public DataEndpointValue ToDataEndpointValue()
    {
        return new DataEndpointValue()
        {
            Owner = Owner,
            Name = Name,
            Type = Type,
            Value = DefaultValue
        };
    }
}