using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            
            // пока пойдет и так
            foreach (var logEvent in events)
            {
                var file = File.Open($"{logEvent.UserId.ToString()}.log", FileMode.Append, FileAccess.Write,
                    FileShare.ReadWrite);
                var writer = new StreamWriter(file);
                await writer.WriteAsync(EventToString(logEvent));
                writer.Close();
            }
        }

        private string EventToString(LogEvent logEvent)
        {
            var parameters = string.Join(";", logEvent.Parameters?.Select(x => $";{x.Key}:{x.Value}") ?? Array.Empty<string>());
            var text = logEvent.Text == null ? "" : $";{logEvent.Text}";
            
            
            return $"{logEvent.DateTime:yyyy-mm-dd hh:MM:ss};{logEvent.Category.ToString()}{text}{parameters}{Environment.NewLine}";
        }
    }
}