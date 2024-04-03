namespace dOSCEngine.Websocket;

public class WebSocketSubscriptionProfile
{
    public Guid Subscriber { get; set; }
    public string Subscription { get; set; } 
    
    public WebSocketSubscriptionProfile(Guid subscriber, string subscription)
    {
        Subscriber = subscriber;
        Subscription = subscription;
    }
}