namespace github.trondr.LogViewer.Library.Module.Services.FileLogItem
{
    public interface IFileLogItemConnectionFactory
    {
        ILogItemConnection GetFileLogItemConnection(string value, string fileName);
        void Release(ILogItemConnection connection);
    }
}