using System.Net;
using github.trondr.LogViewer.Library.Module.Services.TcpLog;

namespace github.trondr.LogViewer.Library.Module.Services.UdpLog
{
    public interface IUdpLogItemConnectionFactory
    {
        ILogItemConnection GetUdpLogItemConnection(string value, string hostname, int port, IpVersion ipVersion);
        ILogItemConnection GetUdpLogItemConnection(string value, string hostname, int port, IpVersion ipVersion, IPAddress multiCastAddress);
        void Release(ILogItemConnection connection);
    }
}