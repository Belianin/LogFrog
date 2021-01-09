using Telegram.Bot.Types.ReplyMarkups;

namespace LogFrog.Telegram.Dialogs
{
    public class StartNode : IDialogNode
    {
        public string Text { get; }
        public IReplyMarkup Markup { get; }
        public IDialogNode Reply(string text)
        {
            throw new System.NotImplementedException();
        }
    }
}