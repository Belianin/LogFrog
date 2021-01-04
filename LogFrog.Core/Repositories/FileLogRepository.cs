using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace LogFrog.Core.Repositories
{
    public class FileLogRepository : ILogRepository
    {
        private readonly Queue<LogEvent> events = new();
        private Task taskLoop;

        public FileLogRepository()
        {
            taskLoop = Task.Run(StartWritingLoopAsync);
        }
        
        public void SaveLogEvent(LogEvent logEvent)
        {
            events.Enqueue(logEvent);
        }

        private async Task StartWritingLoopAsync()
        {
            while (true)
            {
                await Task.Delay(1000).ConfigureAwait(false);
                await WriteEventToFilesAsync().ConfigureAwait(false);
            }
        }
        
        private async Task WriteEventToFilesAsync()
        {
            if (events.Count == 0)
                return;

            // todo не оч конечно
            foreach (var logEvent in events)
            {
                File.AppendText($"{logEvent.Id.ToString()}.log.txt");
            }
        }
    }
}