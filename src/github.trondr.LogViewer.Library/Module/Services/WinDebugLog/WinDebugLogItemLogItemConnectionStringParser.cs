namespace github.trondr.LogViewer.Library.Module.Services.WinDebugLog
{

    public class WinDebugLogItemLogItemConnectionStringParser : IWinDebugLogItemConnectionStringParser
    {
        private readonly IWinDebugLogItemConnectionFactory _winDebugLogItemConnectionFactory;

        public WinDebugLogItemLogItemConnectionStringParser(IWinDebugLogItemConnectionFactory winDebugLogItemConnectionFactory)
        {
            _winDebugLogItemConnectionFactory = winDebugLogItemConnectionFactory;
        }

        public bool CanParse(string connectionString)
        {
            return IsWinDebugConnectionString(connectionString);
        }

        public ILogItemConnection Parse(string connectionString)
        {
            return _winDebugLogItemConnectionFactory.GetWinDebugLogItemConnection(connectionString);
        }

        private bool IsWinDebugConnectionString(string connectionString)
        {
            return connectionString.StartsWith("windebug");
        }
    }
}