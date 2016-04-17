using github.trondr.LogViewer.Library.Module.Services.TcpLog;

namespace github.trondr.LogViewer.Library.Module.Services.UdpLog
{
    public class UdpLogItemConnection : IUdpLogItemConnection
    {
        public string Value { get; set; }
        public string HostName { get; set; }
        public int Port { get; set; }
        public IpVersion IpVersion { get;set; }
    }
}