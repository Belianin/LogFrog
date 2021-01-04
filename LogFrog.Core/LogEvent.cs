using System;

namespace LogFrog.Core
{
    public class LogEvent
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime DateTime { get; set; }
        public LogEventCategory Category { get; set; }
    }
}