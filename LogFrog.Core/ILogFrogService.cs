using System;
using System.Threading.Tasks;

namespace LogFrog.Core
{
    public interface ILogFrogService
    {
        void Log(LogEvent logEvent);
    }
}