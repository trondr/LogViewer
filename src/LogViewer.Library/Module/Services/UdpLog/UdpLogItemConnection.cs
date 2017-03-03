using System.Net;
using LogViewer.Library.Module.Services.TcpLog;

namespace LogViewer.Library.Module.Services.UdpLog
{
    public class UdpLogItemConnection : IUdpLogItemConnection
    {
        public string Value { get; set; }
        public string HostName { get; set; }
        public int Port { get; set; }
        public IpVersion IpVersion { get;set; }
        public IPAddress MultiCastAddress { get; set; }
    }
}