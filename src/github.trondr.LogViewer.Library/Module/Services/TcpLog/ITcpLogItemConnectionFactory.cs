namespace github.trondr.LogViewer.Library.Module.Services.TcpLog
{
    public interface ITcpLogItemConnectionFactory
    {
        ILogItemConnection GetTcpLogItemConnection(string value, string hostname, int port, IpVersion ipVersion);
        void Release(ILogItemConnection connection);
    }
}