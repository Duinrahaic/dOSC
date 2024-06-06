using System;
using Microsoft.VisualBasic.CompilerServices;

namespace dOSC.Shared.Models.Commands;

public class DataEndpoint : Data
{
    public string Owner { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Alias { get; set; } = string.Empty;
    public DataType Type { get; set; } = DataType.Text;
    public string Value { get; set; } = string.Empty;
    public Permissions Permission { get; set; } = Permissions.ReadWrite;
    
    public DataLabels Labels { get; set; } = new();
    public Constraints Constraints { get; set; } = new();
    
    public override string ToString()
    {
        return $"Owner: {Owner}, Name: {Name}, Alias: {Alias}, Type: {Type}, Value: {Value}, Permission: {Permission}";
    }

    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        if (obj is DataEndpoint ep)
        {
            return Owner == ep.Owner && Name == ep.Name;
        }

        return false;
    }
    
    public override int GetHashCode()
    {
        return HashCode.Combine(Owner, Name);
    }
    
    public void UpdateValue(string value) => Value = value;
    
    public void UpdateValue(bool value) => Value = value.ToString();
        
    public void UpdateValue(decimal value) => Value = value.ToString();
    
    
}