using LogFrog.Core.Repositories;

namespace LogFrog.Core
{
    public class FrogLogService : ILogService
    {
        private readonly ILogRepository repository;

        public FrogLogService(ILogRepository repository)
        {
            this.repository = repository;
        }

        public void Log(LogEvent logEvent)
        {
            repository.SaveLogEvent(logEvent);
        }
    }
}