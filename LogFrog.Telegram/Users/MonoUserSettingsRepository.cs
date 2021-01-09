using System.Collections.Generic;
using LogFrog.Core;

namespace LogFrog.Telegram.Users
{
    public class MonoUserSettingsRepository : IUserSettingRepository
    {
        public UserSettings GetSettings(int userId)
        {
            return new UserSettings
            {
                Categories = new List<LogEventCategory>
                {
                    LogEventCategory.Cigarette
                }
            };
        }
    }
}