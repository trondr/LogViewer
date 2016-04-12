namespace github.trondr.LogViewer.Library.Module.Services.WinDebugLog
{
    public interface IWinDebugLogItemConnectionFactory
    {
        ILogItemConnection GetWinDebugLogItemConnection(string value);
        void Release(ILogItemConnection connection);
    }
}