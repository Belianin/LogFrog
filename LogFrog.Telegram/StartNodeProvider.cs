using System.Collections.Generic;
using System.Linq;
using LogFrog.Core;
using LogFrog.Telegram.Dialogs;
using LogFrog.Telegram.Users;

namespace LogFrog.Telegram
{
    public class StartNodeProvider : IStartNodeProvider
    {
        private readonly ILogService logService;
        private readonly IUserSettingRepository settingRepository;
        private readonly Dictionary<LogEventCategory, (string text, IDialogNode node)> nodeSettings;

        public StartNodeProvider(IUserSettingRepository settingRepository, ILogService logService)
        {
            this.settingRepository = settingRepository;
            this.logService = logService;
            
            nodeSettings = new Dictionary<LogEventCategory, (string text, IDialogNode node)>
            {
                [LogEventCategory.Cigarette] = ("🚬 сигареты", new CigaretteNode(logService)),
                [LogEventCategory.Payment] = ("💲 платежи", new MoneyNode(logService)),
                [LogEventCategory.Food] = ("🍴 пища", new FoodNode(logService))
            };
        }

        public StartNode GetStartNode(int userId)
        {
            var settings = settingRepository.GetSettings(userId);
            
            return new StartNode(settings.Categories
                .ToDictionary(k => nodeSettings[k].text,
                    k => nodeSettings[k].node),
                logService);
        }
    }
}