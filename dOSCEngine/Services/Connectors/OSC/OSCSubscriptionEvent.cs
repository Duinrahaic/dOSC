namespace dOSCEngine.Services.Connectors.OSC
{
    public class OSCSubscriptionEvent : EventArgs
    {
        public string Address { get; set; }
        public object[] Arguments { get; set; }
        public OSCSubscriptionEvent(string Address, params object[] args)
        {
            this.Address = Address;
            Arguments = args;
        }
    }
}
