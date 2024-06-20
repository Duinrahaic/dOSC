using System.Globalization;
using dOSC.Client.Models.Commands;
using LiteDB;
using LiveSheet;

namespace dOSC.Drivers.Hub;

public partial class HubService
{
    public delegate void EndpointUpdated(DataEndpoint endpoint);

    public EndpointUpdated? OnEndpointUpdate;
    public delegate void EndpointValueUpdated(DataEndpoint endpoint, DataEndpointValue value);

    public EndpointValueUpdated? OnEndpointValueUpdate;
    
    private static HashSet<DataEndpoint> _endpoints = new();

    public int GetEndpointCount() => _endpoints.Count;
    public int GetEndpointSourcesCount() => _endpoints.Select(e => e.Owner).Distinct().Count();
    public void ClearEndpoints() => _endpoints.Clear();
    public bool RegisterEndpoint(DataEndpoint e)
    {
        var log = EndpointLog;
        bool success = _endpoints.Add(e);
        if (success)
        {
            log.Message = $"Registered ConfigEndpoint: {e.Name} for {e.Owner} ";
            log.Level = DoscLogLevel.Info;
            OnEndpointUpdate?.Invoke(e);
        }
        else
        {
            log.Message = $"Unable to register ConfigEndpoint: {e.Name} for {e.Owner} ";
            log.Level = DoscLogLevel.Warning;
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
            OnEndpointUpdate?.Invoke(e);
            log.Message = $"{e.Owner} unregistered ConfigEndpoint: {e.Name}";
            log.Level = DoscLogLevel.Info;
        }
        else
        {
            log.Message = $"Unable to unregister ConfigEndpoint: {e.Name} for {e.Owner}";
            log.Level = DoscLogLevel.Warning;
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
            log.Level = DoscLogLevel.Warning;
            success = false;
        }
        Log(log);
        return success;
    }
    public DataEndpointValue GetEndpointValue(DataEndpoint e)
    {
        var endpoint = _endpoints.FirstOrDefault(ep => ep.Equals(e));
        if(endpoint != null)
        {
            return endpoint.ToDataEndpointValue();
        }
        return null;
    }
    
    public DataEndpoint GetEndpoint(Subscription subscription)
    {
        return _endpoints.FirstOrDefault(e => e.Owner == subscription.Owner && e.Name == subscription.Address) ?? new DataEndpoint();
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
                log.Level = DoscLogLevel.Warning;
                //Log(log);
                return false;
            }
            
            
            success = true;

            endpoint.DefaultValue = value.Value;
            log.Message = $"Updated ConfigEndpoint {endpoint.Name} for {endpoint.Owner} with value {endpoint.GetDisplayValue()}";
            log.Level = DoscLogLevel.Info;
            //Log(log);
            OnEndpointValueUpdate?.Invoke(endpoint, value);
            SubscriptionService.UpdateSubscription(endpoint);
            return success;
        }
        else
        {
            log.Message = $"ConfigEndpoint {value.Name} for {value.Owner} does not exist to update";
            log.Level = DoscLogLevel.Warning;
            success = false;
            //Log(log);
            return success;
        }
    }
    
    
    

    private static Log EndpointLog => new()
    {
        Origin ="Hub:ConfigEndpoint Service",
        TimeStamp = DateTime.Now.ToString(CultureInfo.CurrentCulture)
    };
    
    
    public List<DataEndpoint> GetEndpoints() => _endpoints.ToList();
    public List<DataEndpoint> GetEndpointsByOrigin(string origin) => _endpoints.Where(e => e.Owner == origin).ToList();
    public List<DataEndpoint> GetEndpointsByAddress(string address) => _endpoints.Where(e => e.Name == address).ToList();

    
}