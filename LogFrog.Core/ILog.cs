using System;
using System.Threading.Tasks;

namespace LogFrog.Core
{
    public interface ILog
    {
        void Log(LogEvent logEvent);
    }
}