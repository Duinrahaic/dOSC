using System.Collections.Concurrent;

namespace dOSC.Middlewear;

public partial class WebSocketMiddleware
{
    private readonly ConcurrentStack<Subscription> _subscriptions = new();


    public void AddSubscription(Subscription newSubscription)
    {
        if (!_subscriptions.Any(s => s.Subscriber == newSubscription.Subscriber)) _subscriptions.Push(newSubscription);
    }

    public void RemoveSubscription(Guid subscriptionId)
    {
        var existingSubscription = _subscriptions.FirstOrDefault(s => s.Subscriber == subscriptionId);
        if (existingSubscription != null) _subscriptions.TryPop(out _);
    }

    public void BulkRemoveSubscriptions(IEnumerable<Guid> subscriptionIdsToRemove)
    {
        foreach (var id in subscriptionIdsToRemove)
        {
            var existingSubscription = _subscriptions.FirstOrDefault(s => s.Subscriber == id);
            if (existingSubscription != null)
            {
                _subscriptions.TryPop(out _);
            }
        }
    }
}