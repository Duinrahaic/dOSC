using System.Collections.Concurrent;

namespace dOSCEngine.Websocket;

public partial class WebSocketMiddleware
{
    private ConcurrentStack<WebSocketSubscriptionProfile> _subscriptions = new();
    
    
    public void AddSubscription(WebSocketSubscriptionProfile subscription)
    {
        _subscriptions.
    }

    public void RemoveSubscription(WebSocketSubscriptionProfile subscription)
    {
        _subscriptions.TryTake()
    }

    public void BulkRemoveSubscriptions(IEnumerable<WebSocketSubscriptionProfile> subscriptionsToRemove)
    {
        foreach (var subscription in subscriptionsToRemove)
        {
            _subscriptions.TryTake(out _, subscription);
        }
    }
    
}

