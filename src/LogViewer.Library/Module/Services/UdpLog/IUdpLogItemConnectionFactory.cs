using System.Net;
using LogViewer.Library.Module.Services.TcpLog;

namespace LogViewer.Library.Module.Services.UdpLog
{
    public interface IUdpLogItemConnectionFactory
    {
        ILogItemConnection GetUdpLogItemConnection(string value, string hostname, int port, IpVersion ipVersion);
        ILogItemConnection GetUdpLogItemConnection(string value, string hostname, int port, IpVersion ipVersion, IPAddress multiCastAddress);
        void Release(ILogItemConnection connection);
    }
}