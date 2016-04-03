namespace github.trondr.LogViewer.Library.Services.EventLogItem
{
    public interface IEventLogItemConnection :ILogItemConnection
    {
        string LogName {get;set; }

        string Machine {get;set; }

        string Source {get;set; }        
    }
}