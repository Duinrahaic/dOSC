using System.Reflection;
using dOSC.Client.Models.Commands;
using LiteDB;

namespace dOSC.Attributes;

public static class EndpointHelper
{
    public static List<DataEndpoint> GetEndpoints(object obj)
    {
        List<DataEndpoint> endpoints = new();
        Type type = obj.GetType();
        PropertyInfo[] properties = type.GetProperties();

        foreach (var property in properties)
        {
            var attribute = property.GetCustomAttribute<ConfigEndpoint>(false);
            if (attribute != null)
            {
                DataEndpoint endpoint = new();
                endpoint.Owner = attribute.Owner;
                endpoint.Name = attribute.Name;
                endpoint.Alias = attribute.Alias;
                endpoint.Description = attribute.Description;
                endpoint.Type = attribute.DataType;
                endpoint.Permissions = attribute.Permissions;
                
                if (attribute is ConfigNumericEndpoint numericAttribute)
                {
                    endpoint.DefaultValue = numericAttribute.DefaultValue.ToString();
                    endpoint.Constraints = new Constraints();
                    
                    endpoint.Constraints.Min = numericAttribute.SafeDoubleToDecimal(numericAttribute.MinValue);
                    endpoint.Constraints.Max =  numericAttribute.SafeDoubleToDecimal(numericAttribute.MaxValue);
                    endpoint.Constraints.Precision = numericAttribute.Precision;
                    endpoint.Labels = new NumericDataLabels()
                    {
                        Unit = numericAttribute.Unit,
                    };
                }
                else if (attribute is ConfigLogicEndpoint logicAttribute)
                {
                    endpoint.DefaultValue = logicAttribute.DefaultValue.ToString();
                    endpoint.Labels = new LogicDataLabels()
                    {
                        TrueLabel = logicAttribute.TrueLabel,
                        FalseLabel = logicAttribute.FalseLabel
                    };
                }
                else if(attribute is ConfigTextEndpoint textAttribute)
                {
                    endpoint.DefaultValue = textAttribute.DefaultValue;
                }
                endpoints.Add(endpoint);
            }
        }
        
        return endpoints;
    }

    public static DataEndpoint? GetDefaultEndpoint(object obj, string name)
    {
        var endpoints = GetEndpoints(obj);
        return endpoints.FirstOrDefault(x => x.Name == name);
    }

    public static DataEndpoint? GetCurrentEndpoint(object obj, string name)
    {
        var value = GetEndpointPropertyValue(obj, name);

        DataEndpoint? ep = GetDefaultEndpoint(obj, name);
        if (ep != null)
            ep.DefaultValue = value?.ToString();

        return ep;
    }

    public static object GetEndpointPropertyValue(object obj, string name)
    {
        Type type = obj.GetType();

        PropertyInfo? selectedProperty = type.GetProperties()
            .FirstOrDefault(prop => prop.GetCustomAttribute<ConfigEndpoint>(false)?.Name == name);

        if (selectedProperty == null)
            return null;
        return selectedProperty.GetValue(obj);
    }


    public static bool TryUpdateEndpointProperty(object obj, DataEndpoint endpoint, BsonValue value)
    {
        // Get the type of the object
        Type type = obj.GetType();
    
        // Find the property that matches the endpoint name
        
        PropertyInfo? selectedProperty = type.GetProperties()
            .FirstOrDefault(prop => prop.GetCustomAttribute<ConfigEndpoint>(false)?.Name == endpoint.Name);
        
        if (selectedProperty == null)
        {
            return false;
        }
    
        // Convert the BsonValue to the type required by the property
        object? convertedValue = null;
        if (selectedProperty.PropertyType == typeof(int))
        {
            convertedValue = value.AsInt32;
        }
        else if (selectedProperty.PropertyType == typeof(decimal))
        {
            convertedValue = value.AsDecimal;
        }
        else if (selectedProperty.PropertyType == typeof(bool))
        {
            convertedValue = value.AsBoolean;
        }
        else if (selectedProperty.PropertyType == typeof(string))
        {
            convertedValue = value.AsString;
        }
        else if (selectedProperty.PropertyType.IsEnum)
        {
            convertedValue = Enum.Parse(selectedProperty.PropertyType, value.AsString);
        }
        else
        {
            return false;
        }

        // Set the property value
        try
        {
            selectedProperty.SetValue(obj, convertedValue);
        }
        catch
        {
            return false;
        }
        return true;
    }
        
}