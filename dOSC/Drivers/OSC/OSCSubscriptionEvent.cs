using System;
using System.Collections.Generic;

namespace dOSC.Drivers.OSC;

public class OSCSubscriptionEvent : EventArgs
{
    public OSCSubscriptionEvent(string Address, List<object> args)
    {
        this.Address = Address;
        Arguments = args;
    }

    public string Address { get; set; }
    public List<object> Arguments { get; set; }
}