namespace github.trondr.LogViewer.Library.Services.RandomLogItem
{
    public interface IRandomLogItemConnectionFactory
    {
        ILogItemConnection GetRandomLogItemConnection(string value);
        void Release(ILogItemConnection connection);
    }
}