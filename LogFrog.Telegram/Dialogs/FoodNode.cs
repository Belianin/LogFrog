using System;
using System.Collections.Generic;
using System.Linq;
using LogFrog.Core;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace LogFrog.Telegram.Dialogs
{
    public class FoodNode : IDialogNode
    {
        private readonly ILogService logService;
        private readonly HashSet<string> categories = new HashSet<string>
        {
            "Обычная еда",
            "Сладкое",
            "Кофе/Чай",
            "Фаст-фуд",
        };
        
        public string Text => "Введите сумму или выберите категорию";
        public IReplyMarkup Markup => new ReplyKeyboardMarkup(categories.Select(c => new KeyboardButton(c)));

        public FoodNode(ILogService logService)
        {
            this.logService = logService;
        }

        public IDialogNode? Reply(Message message)
        {
            var logEvent = new LogEvent
            {
                DateTime = DateTime.Now,
                Category = LogEventCategory.Food,
                Parameters = new Dictionary<string, object>
                {
                    ["Category"] = message.Text
                },
                UserId = message.From.Id
            };

            return new WithTextNode(logEvent, logService);
        }
    }
}