namespace dOSC.Drivers.Hub;

public class Subscription
{
    public string Subscriber { get; set; }
    public string Owner { get; set; }
    public string Address { get; set; }
    
    public Subscription(string subscriber, string owner, string address)
    {
        Subscriber = subscriber;
        Owner = owner;
        Address = address;
    }
}