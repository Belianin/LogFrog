using System;

namespace LogFrog.Core
{
    public class LogEvent
    {
        public long UserId { get; set; }
        public string Text { get; set; }
        public DateTime DateTime { get; set; }
        public LogEventParameters Parameters { get; set; }
    }
}