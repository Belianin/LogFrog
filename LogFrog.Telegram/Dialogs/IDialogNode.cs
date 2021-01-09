using Telegram.Bot.Types.ReplyMarkups;

namespace LogFrog.Telegram.Dialogs
{
    public interface IDialogNode
    {
        string Text { get; }
        IReplyMarkup Markup { get; }
        IDialogNode Reply(string text);
    }
}