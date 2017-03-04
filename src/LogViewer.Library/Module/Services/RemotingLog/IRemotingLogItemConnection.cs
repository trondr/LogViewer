namespace LogViewer.Library.Module.Services.RemotingLog
{
    public interface IRemotingLogItemConnection : ILogItemConnection
    {
        string SinkName {get;set; }

        int Port {get;set; }
                
    }
}