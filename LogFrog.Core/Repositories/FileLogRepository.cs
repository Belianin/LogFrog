using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace LogFrog.Core.Repositories
{
    public class FileLogRepository : ILogRepository
    {
        private readonly Queue<LogEvent> events = new Queue<LogEvent>();
        private readonly Task taskLoop;

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
            
            foreach (var logEvent in events)
            {
                var file = File.AppendText($"{logEvent.UserId.ToString()}.log.txt");
                await file.WriteLineAsync($"{logEvent.DateTime:yyyy-mm-dd hh:MM:ss} [{logEvent.Category.ToString().ToUpper()}] {logEvent.Text}");
            }
        }
    }
}