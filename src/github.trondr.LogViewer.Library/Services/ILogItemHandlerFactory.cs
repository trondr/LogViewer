namespace github.trondr.LogViewer.Library.Services
{
    public interface ILogItemHandlerFactory
    {
        ILogItemHandler[] GetLogItemHandlers(ILogItemConnection connection);
    }
}