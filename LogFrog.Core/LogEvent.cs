using System;
using System.Collections.Generic;

namespace LogFrog.Core
{
    public class LogEvent
    {
        public int UserId { get; set; }
        public string Text { get; set; }
        public DateTime DateTime { get; set; }
        public LogEventCategory Category { get; set; }
        public Dictionary<string, object> Parameters { get; set; }
    }
}