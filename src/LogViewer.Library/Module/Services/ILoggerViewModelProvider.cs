using LogViewer.Library.Module.ViewModels;

namespace LogViewer.Library.Module.Services
{
    public interface ILoggerViewModelProvider
    {
        LoggerViewModel GetLogger(string logger);

        LoggerViewModel Root { get; }
    }
}