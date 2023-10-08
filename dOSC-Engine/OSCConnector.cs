using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VRC.OSCQuery;

namespace dOSC_Engine
{
    public class OSCConnector : BackgroundWorker, IDisposable
    {
        private OSCQueryService? _OSC; 
        public OSCConnector(IServiceProvider service)
        {
            _OSC = new OSCQueryServiceBuilder().WithDefaults().Build();
            IDiscovery discovery = new MeaModDiscovery();
            
        }

        
        


        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
