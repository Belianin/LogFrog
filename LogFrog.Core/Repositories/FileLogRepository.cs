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
            
            var groupedEvents = new Dictionary<int, List<LogEvent>>();

            while (events.Count != 0)
            {
                var logEvent = events.Dequeue();
                if (!groupedEvents.ContainsKey(logEvent.UserId))
                    groupedEvents[logEvent.UserId] = new List<LogEvent>();
                
                groupedEvents[logEvent.UserId].Add(logEvent);
            }

            foreach (var (userId, eventList) in groupedEvents)
            {
                var file = File.Open($"{userId.ToString()}.log", FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
                var writer = new StreamWriter(file);
                var line = string.Join(Environment.NewLine, eventList.Select(EventToString));
                await writer.WriteLineAsync(line);
                writer.Close();
            }
        }

        private string EventToString(LogEvent logEvent)
        {
            var parameters = logEvent.Parameters == null || logEvent.Parameters.Count == 0
                ? ""
                : $"{string.Join(",", logEvent.Parameters.Select(x => $"{x.Key}={x.Value}"))}";
            var text = logEvent.Text ?? "";

            return $"{logEvent.DateTime:yyyy-MM-dd HH:mm:ss};{logEvent.Category.ToString()};{text};{parameters}";
        }
    }
}