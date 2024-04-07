namespace dOSC.Hub.Middlewear;

public class Subscription
{
    public Guid Subscriber { get; set; }
    public string Feed { get; set; } 
    
    public Subscription(Guid subscriber, string feed)
    {
        Subscriber = subscriber;
        Feed = feed;
    }
}