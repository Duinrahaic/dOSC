namespace dOSC.Client.Services.Connectors.Hub.OSC
{
    public class OSCSubscriptionEvent : EventArgs
    {
        public string Address { get; set; }
        public List<object> Arguments { get; set; }
        public OSCSubscriptionEvent(string Address, List<object> args)
        {
            this.Address = Address;
            Arguments = args;
        }



    }
}
