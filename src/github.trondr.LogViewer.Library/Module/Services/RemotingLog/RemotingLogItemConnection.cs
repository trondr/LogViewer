namespace github.trondr.LogViewer.Library.Module.Services.RemotingLog
{
    public class RemotingLogItemConnection : IRemotingLogItemConnection
    {
        public string Value { get; set; }
        public string SinkName { get; set; }
        public int Port { get; set; }        
    }
}