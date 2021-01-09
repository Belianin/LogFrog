using System;
using System.Collections.Generic;
using LogFrog.Core;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace LogFrog.Telegram.Dialogs
{
    public class CigaretteNode : IDialogNode
    {
        private readonly ILogService logService;
        public string Text => "Сколько?";
        public IReplyMarkup Markup => new ReplyKeyboardMarkup(new KeyboardButton("1"));
        
        public CigaretteNode(ILogService logService)
        {
            this.logService = logService;
        }
        
        public IDialogNode? Reply(Message message)
        {
            if (!int.TryParse(message.Text, out var count))
                count = 1;
            
            logService.Log(new LogEvent
            {
                Category = LogEventCategory.Cigarette,
                DateTime = DateTime.Now,
                UserId = message.From.Id,
                Parameters = new Dictionary<string, object>
                {
                    ["Count"] = count
                }
            });

            return null;
        }
    }
}