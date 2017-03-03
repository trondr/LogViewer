using System;

namespace LogViewer.Library.Module.Model
{
    public interface ILogItemFactory
    {
        LogItem GetLogItem(DateTime time, LogLevel logLevel, string logger, string threadId, string message, string exceptionString);

        void Release(LogItem logItem);
    }
}