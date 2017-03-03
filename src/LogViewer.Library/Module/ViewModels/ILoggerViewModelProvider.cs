namespace LogViewer.Library.Module.ViewModels
{
    public interface ILoggerViewModelProvider
    {
        LoggerViewModel GetLogger(string logger);

        LoggerViewModel Root { get; }
    }
}