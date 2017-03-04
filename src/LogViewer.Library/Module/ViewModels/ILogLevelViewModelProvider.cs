namespace LogViewer.Library.Module.ViewModels
{
    public interface ILogLevelViewModelProvider
    {
        LogLevelViewModel GetLevel(string logLevel);
    }
}