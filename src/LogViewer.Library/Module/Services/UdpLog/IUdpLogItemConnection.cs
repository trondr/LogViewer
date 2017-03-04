using System.Net;
using LogViewer.Library.Module.Services.TcpLog;

namespace LogViewer.Library.Module.Services.UdpLog
{
    public interface IUdpLogItemConnection : ILogItemConnection
    {        
        string HostName { get; set; }
        int Port { get; set; }
        IpVersion IpVersion { get;set; }
        IPAddress MultiCastAddress { get; set; }        
    }
}