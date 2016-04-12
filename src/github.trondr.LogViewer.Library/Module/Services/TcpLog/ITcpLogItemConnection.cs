namespace github.trondr.LogViewer.Library.Module.Services.TcpLog
{
    public interface ITcpLogItemConnection : ILogItemConnection
    {        
        string HostName { get; set; }
        int Port { get; set; }
        IpVersion IpVersion { get;set; }
    }
}