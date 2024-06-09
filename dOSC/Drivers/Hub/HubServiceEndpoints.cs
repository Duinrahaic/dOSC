using System.Globalization;
using dOSC.Client.Models.Commands;
using LogLevel = dOSC.Client.Models.Commands.LogLevel;

namespace dOSC.Drivers;

public partial class HubService
{
    private HashSet<DataEndpoint> _endpoints = new();

    public bool RegisterEndpoint(DataEndpoint e)
    {
        var log = EndpointLog;
        bool success = _endpoints.Add(e);
        if (success)
        {
            log.Message = $"Registered ConfigEndpoint: {e.Name} for {e.Owner} ";
            log.Level = LogLevel.Info;
        }
        else
        {
            log.Message = $"Unable to register ConfigEndpoint: {e.Name} for {e.Owner} ";
            log.Level = LogLevel.Warning;
        }
        Log(log);
        return success;
    } 
    public bool UnregisterEndpoint(DataEndpoint e)
    {
        var log = EndpointLog;
        bool success = _endpoints.Remove(e);
        if (success)
        {
            log.Message = $"{e.Owner} unregistered ConfigEndpoint: {e.Name}";
            log.Level = LogLevel.Info;
        }
        else
        {
            log.Message = $"Unable to unregister ConfigEndpoint: {e.Name} for {e.Owner}";
            log.Level = LogLevel.Warning;
        }
        Log(log);
        return success;
    }
    public bool IsRegistered(DataEndpoint e) => _endpoints.Contains(e);
    public bool UpdateEndpoint(DataEndpoint e)
    {
        var log = EndpointLog;
        bool success = false;
        if(IsRegistered(e))
        {
            var endpoint = _endpoints.First(ep => ep.Equals(e));
        }
        else
        {
            log.Message = $"ConfigEndpoint {e.Name} for {e.Owner} does not exist to update";
            log.Level = LogLevel.Warning;
            success = false;
        }
        Log(log);
        return success;
    }
    
    public bool UpdateEndpointValue(DataEndpointValue value)
    {
        var log = EndpointLog;
        bool success = false;
        var endpoint = _endpoints.FirstOrDefault(e => e.Name == value.Name && e.Owner == value.Owner);
        if(endpoint != null)
        {
            if(endpoint.Type != value.Type)
            {
                log.Message = $"ConfigEndpoint {value.Name} for {value.Owner} has different type than the value provided";
                log.Level = LogLevel.Warning;
                Log(log);
                return false;
            }
            
            
            success = true;
            log.Message = $"Updated ConfigEndpoint {endpoint.Name} for {endpoint.Owner} with value {value.Value}";
            log.Level = LogLevel.Info;
        }
        else
        {
            log.Message = $"ConfigEndpoint {value.Name} for {value.Owner} does not exist to update";
            log.Level = LogLevel.Warning;
            success = false;
        }
        Log(log);
        return success;
    }
    

    private static Log EndpointLog => new()
    {
        Origin ="Hub:ConfigEndpoint Service",
        TimeStamp = DateTime.Now.ToString(CultureInfo.CurrentCulture)
    };
    
    
    public List<DataEndpoint> GetEndpoints() => _endpoints.ToList();
    public List<DataEndpoint> GetEndpoints(string origin) => _endpoints.Where(e => e.Owner == origin).ToList();
}