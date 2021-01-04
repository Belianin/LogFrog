namespace LogFrog.Core.Repositories
{
    public interface ILogRepository
    {
        void SaveLogEvent(LogEvent logEvent);
    }
}