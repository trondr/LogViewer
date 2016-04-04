using System;
using System.Collections.Generic;

namespace github.trondr.LogViewer.Library.Module.Model
{
    public interface ILogItemFactory
    {
        LogItem GetLogItem(DateTime time, LogLevel logLevel, string logger, string threadId, string message, string exceptionString, Dictionary<string,string> properties);

        void Release(LogItem logItem);
    }
}