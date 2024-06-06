using System.Globalization;
using dOSC.Shared.Models.Commands;
using LogLevel = dOSC.Shared.Models.Commands.LogLevel;

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
            log.Message = $"Registered Endpoint: {e.Name} for {e.Owner} ";
            log.Level = LogLevel.Info;
        }
        else
        {
            log.Message = $"Unable to register Endpoint: {e.Name} for {e.Owner} ";
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
            log.Message = $"{e.Owner} unregistered Endpoint: {e.Name}";
            log.Level = LogLevel.Info;
        }
        else
        {
            log.Message = $"Unable to unregister Endpoint: {e.Name} for {e.Owner}";
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
            endpoint.UpdateValue(e.Value);
            log.Message = $"{e.Owner} Updated Endpoint: {e.Name} {e.Value} for {e.Owner}";
            log.Level = LogLevel.Info;
            success = true;
        }
        else
        {
            log.Message = $"Endpoint {e.Name} for {e.Owner} does not exist to update";
            log.Level = LogLevel.Warning;
            success = false;
        }
        Log(log);
        return success;
    }

    private static Log EndpointLog => new()
    {
        Origin ="Hub:Endpoint Service",
        TimeStamp = DateTime.Now.ToString(CultureInfo.CurrentCulture)
    };
    
    
    public List<DataEndpoint> GetEndpoints() => _endpoints.ToList();
    public List<DataEndpoint> GetEndpoints(string origin) => _endpoints.Where(e => e.Owner == origin).ToList();
}