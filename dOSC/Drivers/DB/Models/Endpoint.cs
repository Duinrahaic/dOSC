using System;

namespace dOSC.Drivers.DB.Models;

public class Endpoint
{
    public string ServiceName { get; set; }
    public string Parameter { get; set; }
    public DataType Type { get; set; }
    public Policy Policy { get; set; }
    public Facet? Facet { get; set; }
    public string? Value { get; set; } 
    public DateTime LastUpdated { get; set; }
    
    public Endpoint(string serviceName, string parameter,string value) // User For Updates
    {
        ServiceName = serviceName;
        Parameter = parameter;
        Value = value;
        LastUpdated = DateTime.Now.ToUniversalTime();
    }
    
    public Endpoint(string serviceName, string parameter, DataType type, Policy policy, Facet? facet, string? value) // Used For Registration
    {
        ServiceName = serviceName;
        Parameter = parameter;
        Type = type;
        Policy = policy;
        Facet = facet;
        Value = value;
        LastUpdated = DateTime.Now.ToUniversalTime();
    }
}