namespace github.trondr.LogViewer.Library.Module.Services.TcpLog
{
    public class TcpLogItemConnection : ITcpLogItemConnection
    {
        public string Value { get; set; }
        public string HostName { get; set; }
        public int Port { get; set; }
        public IpVersion IpVersion { get;set; }
    }
}