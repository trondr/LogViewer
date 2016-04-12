namespace github.trondr.LogViewer.Library.Module.Services.EventLog
{
    public interface IEventLogItemConnection : ILogItemConnection
    {
        string LogName {get;set; }

        string Machine {get;set; }

        string Source {get;set; }        
    }
}