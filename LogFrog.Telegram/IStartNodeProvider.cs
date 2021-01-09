using LogFrog.Telegram.Dialogs;

namespace LogFrog.Telegram
{
    public interface IStartNodeProvider
    {
        StartNode GetStartNode(int userId);
    }
}