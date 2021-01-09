using System.Collections.Generic;
using LogFrog.Core;

namespace LogFrog.Telegram.Users
{
    public class UserSettings
    {
        public ICollection<LogEventCategory> Categories { get; set; }
    }
}