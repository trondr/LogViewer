namespace github.trondr.LogViewer.Library.ViewModels
{
    public interface ILogLevelViewModelProvider
    {
        LogLevelViewModel GetLevel(string logLevel);
    }
}