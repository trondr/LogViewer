using System;

namespace LogViewer.Library.Module.Model
{
    public interface ILogItem
    {
        DateTime Time { get; set; }
        LogLevel LogLevel { get; set; }
        string Logger { get; set; }
        string ThreadId { get; set; }
        string Message { get; set; }
        string ExceptionString { get; set; }
    }
}