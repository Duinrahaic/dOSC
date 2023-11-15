using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dOSCEngine.Hub
{
    internal class LogEntry
    {
        public DateTime DateTime { get; set; }
        public string Source { get; set; } = string.Empty;
        public string Message { get; set; } = String.Empty; 
        public LogEntry(DateTime dateTime, string source, string message)
        {
            DateTime = dateTime;
            Source = source;
            Message = message;
        }   
    }
}
