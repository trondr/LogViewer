namespace github.trondr.LogViewer.Library.ViewModels
{
    public interface ILoggerViewModelProvider
    {
        LoggerViewModel GetLogger(string logger);

        LoggerViewModel Root { get; }
    }
}