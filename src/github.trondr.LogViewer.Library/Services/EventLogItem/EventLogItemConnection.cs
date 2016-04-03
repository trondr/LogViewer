namespace github.trondr.LogViewer.Library.Services.EventLogItem
{
    public class EventLogItemConnection : IEventLogItemConnection
    {
        public string Value { get; set; }
        public string LogName { get; set; }
        public string Machine { get; set; }
        public string Source { get; set; }
    }
}