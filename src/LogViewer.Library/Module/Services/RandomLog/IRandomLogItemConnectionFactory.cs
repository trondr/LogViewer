namespace LogViewer.Library.Module.Services.RandomLog
{
    public interface IRandomLogItemConnectionFactory
    {
        ILogItemConnection GetRandomLogItemConnection(string value);
        void Release(ILogItemConnection connection);
    }
}