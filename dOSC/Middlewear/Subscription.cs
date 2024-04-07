namespace dOSC.Middlewear;

public class Subscription
{
    public Subscription(Guid subscriber, string feed)
    {
        Subscriber = subscriber;
        Feed = feed;
    }

    public Guid Subscriber { get; set; }
    public string Feed { get; set; }
}