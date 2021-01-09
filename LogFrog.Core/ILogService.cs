using System;
using System.Threading.Tasks;

namespace LogFrog.Core
{
    public interface ILogService
    {
        void Log(LogEvent logEvent);
    }
}