namespace github.trondr.LogViewer.Library.Services.FileLogItem
{
    public interface IFileLogItemConnectionFactory
    {
        ILogItemConnection GetFileLogItemConnection(string value);
        void Release(ILogItemConnection connection);
    }
}