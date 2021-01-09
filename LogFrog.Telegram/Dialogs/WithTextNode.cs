using LogFrog.Core;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace LogFrog.Telegram.Dialogs
{
    public class WithTextNode : IDialogNode
    {
        private readonly ILogService logService;
        private readonly LogEvent logEvent;

        public WithTextNode(LogEvent logEvent, ILogService logService)
        {
            this.logEvent = logEvent;
            this.logService = logService;
        }

        public string Text => "Комментарий?";
        public IReplyMarkup Markup => new ReplyKeyboardMarkup(new KeyboardButton("Без комментариев"));
        public IDialogNode? Reply(Message message)
        {
            if (message.Text != "Без комментариев")
                logEvent.Text = message.Text;

            logService.Log(logEvent);

            return null;
        }
    }
}