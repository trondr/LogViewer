using System;
using LogViewer.Library.Module.Common.Collection;
using LogViewer.Library.Module.Services;

namespace LogViewer.Library.Module.ViewModels
{
    public interface ILogItemViewModel
    {
        string ExceptionString { get; set; }
        bool IsVisible { get; set; }
        LoggerViewModel Logger { get; set; }
        LogLevelViewModel LogLevel { get; set; }
        string Message { get; set; }
        ObservableDictionary<string, string> Properties { get; set; }
        string ThreadId { get; set; }
        DateTime Time { get; set; }
        SourceCodeInfo SourceCode { get; set; }
        uint SourceCodeLine { get; set; }
    }
}