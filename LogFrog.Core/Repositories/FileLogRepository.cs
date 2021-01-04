using System.Collections.Generic;
using System.IO;
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

            var files = new Dictionary<int, StreamWriter>();

            StreamWriter GetLogFile(int id)
            {
                if (files.TryGetValue(id, out var file))
                    return file;

                file = File.AppendText($"{id.ToString()}.log.txt");
                files[id] = file;

                return file;
            }
            
            foreach (var logEvent in events)
            {
                await GetLogFile(logEvent.UserId).WriteLineAsync($"{logEvent.DateTime:yyyy-mm-dd hh:MM:ss} [{logEvent.Category.ToString().ToUpper()}] {logEvent.Text}");
            }
        }
    }
}