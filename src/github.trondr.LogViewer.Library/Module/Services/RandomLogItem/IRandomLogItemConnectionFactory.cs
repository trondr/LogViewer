namespace github.trondr.LogViewer.Library.Module.Services.RandomLogItem
{
    public interface IRandomLogItemConnectionFactory
    {
        ILogItemConnection GetRandomLogItemConnection(string value);
        void Release(ILogItemConnection connection);
    }
}