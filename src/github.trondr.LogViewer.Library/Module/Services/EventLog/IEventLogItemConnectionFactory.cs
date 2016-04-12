namespace github.trondr.LogViewer.Library.Module.Services.EventLog
{
    public interface IEventLogItemConnectionFactory
    {
        ILogItemConnection GetEventLogItemConnection(string value, string logName, string machine, string source);
        void Release(ILogItemConnection connection);
    }
}