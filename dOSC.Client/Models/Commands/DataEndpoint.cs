using System;
using System.Globalization;

namespace dOSC.Client.Models.Commands;

public class DataEndpoint : Data
{
    public string Owner { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Alias { get; set; } = string.Empty;
    public DataType Type { get; set; } = DataType.Unknown;
    public string DefaultValue { get; set; } = string.Empty;
    public Permissions Permissions { get; set; } = Permissions.ReadWrite;
    public string Unit { get; set; } = string.Empty;
    public bool System { get; set; } = false;
    
    public DataLabels Labels { get; set; } = new();
    public Constraints Constraints { get; set; } = new();

    

    public string GetName()
    {
        if(string.IsNullOrEmpty(Alias)) return Name;

        return Alias;
    }
    
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


    public string GetDisplayValue()
    {
        if(Labels is NumericDataLabels numLabels)
        {
            
            // Convert to double first to handle scientific notation
            double temp = double.Parse(DefaultValue, CultureInfo.InvariantCulture);
            // Then convert to decimal
            decimal value = (decimal)temp;
            return $"{value.ToString($"F{Constraints.Precision}")} {numLabels.Unit}";
        }
        else if(Labels is LogicDataLabels logicLabels)
        {
            if(DefaultValue.ToLower() == "true" || DefaultValue.ToLower() == "1")
            {
                return logicLabels.TrueLabel;
            }
            else
            {
                return logicLabels.FalseLabel;
            }
        }
        else
        {
            return DefaultValue.ToString();
        }
    }
    
}