namespace github.trondr.LogViewer.Library.Services.EventLogItem
{
    public interface IEventLogItemConnectionFactory
    {
        ILogItemConnection GetEventLogItemConnection(string value, string logName, string machine, string source);
        void Release(ILogItemConnection connection);
    }
}