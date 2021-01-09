using System;
using System.Collections.Generic;
using System.Linq;
using LogFrog.Core;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace LogFrog.Telegram.Dialogs
{
    public class StartNode : IDialogNode
    {
        private readonly ILogService logService;
        private readonly Dictionary<string, IDialogNode> categoryNodes;
        public string Text => "Напишите любое сообщение для логгирования или выберите категорию";
        public IReplyMarkup Markup => new ReplyKeyboardMarkup(
            categoryNodes.Select(x => new KeyboardButton(x.Key)));

        public StartNode(Dictionary<string, IDialogNode> categoryNodes, ILogService logService)
        {
            this.categoryNodes = categoryNodes;
            this.logService = logService;
        }
        
        public IDialogNode Reply(Message message)
        {
            if (categoryNodes.TryGetValue(message.Text, out var reply))
                return reply;
            
            logService.Log(new LogEvent
            {
                DateTime = DateTime.Now,
                Category = LogEventCategory.Info,
                Text = message.Text,
                UserId = message.From.Id
            });

            return this;
        }
    }
}