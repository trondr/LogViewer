using LogViewer.Library.Module.ViewModels;

namespace LogViewer.Library.Module.Services
{
    public interface ILogLevelViewModelProvider
    {
        LogLevelViewModel GetLevel(string logLevel);
    }
}