using LogFrog.Core.Repositories;

namespace LogFrog.Core
{
    public class FrogLog : ILog
    {
        private readonly ILogRepository repository;

        public FrogLog(ILogRepository repository)
        {
            this.repository = repository;
        }

        public void Log(LogEvent logEvent)
        {
            repository.SaveLogEvent(logEvent);
        }
    }
}