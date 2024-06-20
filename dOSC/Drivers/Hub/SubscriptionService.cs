using dOSC.Client.Models.Commands;
using LiteDB;

namespace dOSC.Drivers.Hub;

public static class SubscriptionService
{
     private static HashSet<Subscription> _subscriptions = new HashSet<Subscription>();

    // Custom event delegate to pass Subscription details
    public delegate void SubscriptionEventHandler(string subscriber, string owner, string address);
    public delegate void SubscriptionUpdateEventHandler(Subscription subscription, DataEndpoint endpoint);
    // Events to notify subscribers about subscription changes
    public static event SubscriptionEventHandler SubscriptionAdded;
    public static event SubscriptionEventHandler SubscriptionRemoved;
    public static event SubscriptionUpdateEventHandler SubscriptionUpdated;
    private static Func<Subscription,DataEndpoint> _valueRetriever;
    public static DataEndpoint Subscribe(string subscriber, string owner, string address)
    {
        Subscription subscription = new Subscription(subscriber, owner, address);
        if (!IsSubscribed(subscriber, owner, address))
        {
            _subscriptions.Add(subscription);
            SubscriptionAdded?.Invoke(subscriber, owner, address); // Notify subscribers of the new subscription
        }
        return GetCurrentEndpoint(subscription);
    }

    public static void Unsubscribe(string subscriber, string owner, string address)
    {
        Subscription subscriptionToRemove = _subscriptions.FirstOrDefault(s => s.Subscriber == subscriber && s.Owner == owner && s.Address == address);
        if (subscriptionToRemove != null)
        {
            _subscriptions.Remove(subscriptionToRemove);
            SubscriptionRemoved?.Invoke(subscriber, owner, address); // Notify subscribers of the removed subscription
        }
    }

    public static void SetValueRetriever(Func<Subscription,DataEndpoint> valueRetriever)
    {
        _valueRetriever = valueRetriever;
    }
    
    private static DataEndpoint GetCurrentEndpoint(Subscription subscription)
    {
        if (_valueRetriever != null)
        {
            return _valueRetriever(subscription);
        }
        else
        {
            return DefaultValueRetriever(subscription);
        }
    }

    private static DataEndpoint DefaultValueRetriever(Subscription subscription)
    {
        // Implement your default logic here
        return new DataEndpoint(); // Example of returning a new instance
    }
    
    public static void UpdateSubscription(DataEndpoint endpoint)
    {
        foreach (var subscription in _subscriptions.Where(x=> x.Owner == endpoint.Owner && x.Address == endpoint.GetName()))
        {
            SubscriptionUpdated?.Invoke(subscription, endpoint); // Notify subscribers of the updated subscription with BsonValue
        }
    }
    
    public static bool IsSubscribed(string subscriber, string owner, string address)
    {
        return _subscriptions.Any(s => s.Subscriber == subscriber && s.Owner == owner && s.Address == address);
    }

    public static IEnumerable<Subscription> GetSubscriptions()
    {
        return _subscriptions;
    }
}