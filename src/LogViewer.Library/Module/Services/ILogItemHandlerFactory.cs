namespace LogViewer.Library.Module.Services
{
    public interface ILogItemHandlerFactory
    {
        ILogItemHandler[] GetLogItemHandlers(ILogItemConnection connection);
    }
}