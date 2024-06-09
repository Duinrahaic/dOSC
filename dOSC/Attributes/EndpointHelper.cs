using System.Reflection;
using dOSC.Client.Models.Commands;

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
                endpoints.Add(endpoint);
            }
        }
        
        return endpoints;
    }
}