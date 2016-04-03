namespace github.trondr.LogViewer.Library.Services.FileLogItem
{
    public interface IFileLogItemConnectionFactory
    {
        ILogItemConnection GetFileLogItemConnection(string value, string fileName);
        void Release(ILogItemConnection connection);
    }
}