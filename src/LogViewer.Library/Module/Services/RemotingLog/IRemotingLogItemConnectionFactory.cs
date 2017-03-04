namespace LogViewer.Library.Module.Services.RemotingLog
{
    public interface IRemotingLogItemConnectionFactory
    {
        ILogItemConnection GetRemotingLogItemConnection(string value, string sinkName, string port);
        void Release(ILogItemConnection connection);
    }
}