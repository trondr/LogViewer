using LogViewer.Library.Module.Model;

namespace LogViewer.Library.Module.Services
{
    public interface ILogLevelProvider
    {
        LogLevel GetLogLevel(int logLevelValue);
    }

    public class LogLevelProvider : ILogLevelProvider
    {
        public LogLevel GetLogLevel(int logLevelValue)
        {
            foreach (LogLevelInfo info in LogLevelInfos)
            {
                if (IsInRange(logLevelValue, info.RangeMin, info.RangeMax))
                    return info.Level;
            }
            return LogLevel.None;
        }

        public static bool IsInRange(int val, int min, int max)
        {
            return (val >= min) && (val <= max);
        }

        private LogLevelInfo[] LogLevelInfos
        {
            get
            {
                if (_logLevelInfos == null)
                {
                    _logLevelInfos = new LogLevelInfo[]
                                                        {
                                                            new LogLevelInfo(LogLevel.Trace, 10000, 0, 10000),
                                                            new LogLevelInfo(LogLevel.Debug, 30000, 10001, 30000),
                                                            new LogLevelInfo(LogLevel.Info, 40000, 30001, 40000),
                                                            new LogLevelInfo(LogLevel.Warn, 60000, 40001, 60000),
                                                            new LogLevelInfo(LogLevel.Error,70000, 60001, 70000),
                                                            new LogLevelInfo(LogLevel.Fatal,110000, 70001, 110000),
                                                        };
                }
                return _logLevelInfos;
            }
        }
        private LogLevelInfo[] _logLevelInfos;

    }
}