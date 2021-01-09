using System;
using System.Collections.Generic;
using System.Linq;
using LogFrog.Core;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace LogFrog.Telegram.Dialogs
{
    public class MoneyNode : IDialogNode
    {
        private readonly ILogService logService;
        private readonly HashSet<string> categories = new HashSet<string>
        {
            "Еда",
            "Транспорт",
            "Жилье",
            "Развлечения",
            "Сигареты"
        };
        
        public string Text => "Введите сумму или выберите категорию";
        public IReplyMarkup Markup => new ReplyKeyboardMarkup(categories.Select(c => new KeyboardButton(c)));

        public MoneyNode(ILogService logService)
        {
            this.logService = logService;
        }

        public IDialogNode? Reply(Message message)
        {
            if (decimal.TryParse(message.Text, out var amount))
            {
                logService.Log(new LogEvent
                {
                    Category = LogEventCategory.Payment,
                    DateTime = DateTime.Now,
                    UserId = message.From.Id,
                    Parameters = new Dictionary<string, object>
                    {
                        ["Amount"] = amount
                    },
                });
                return null;
            }
            
            if (categories.TryGetValue(message.Text, out var category))
                return new MoneyWithCategoryNode(logService, category);

            return this;
        }

        private class MoneyWithCategoryNode : IDialogNode
        {
            private readonly ILogService logService;
            private readonly string category;

            public MoneyWithCategoryNode(ILogService logService, string category)
            {
                this.logService = logService;
                this.category = category;
            }

            public string Text => $"Категория: {category}";
            public IReplyMarkup Markup => new ReplyKeyboardRemove();
            public IDialogNode? Reply(Message message)
            {
                if (!decimal.TryParse(message.Text, out var amount))
                    return this;
                
                logService.Log(new LogEvent
                {
                    Category = LogEventCategory.Payment,
                    DateTime = DateTime.Now,
                    Parameters = new Dictionary<string, object>
                    {
                        ["Category"] = category,
                        ["Amount"] = amount
                    },
                    UserId = message.From.Id
                });
                return null;
            }
        }
    }
}