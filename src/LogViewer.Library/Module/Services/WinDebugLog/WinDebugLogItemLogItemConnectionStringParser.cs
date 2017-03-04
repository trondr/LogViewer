namespace LogViewer.Library.Module.Services.WinDebugLog
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
            if(IsWinDebugConnectionString(connectionString))
            {
                return _winDebugLogItemConnectionFactory.GetWinDebugLogItemConnection(connectionString);
            }
            var message = string.Format("Invalid random connection string '{0}'. Valid {1}", connectionString, HelpString);
            throw new InvalidConnectionStringException(message);
        }

        public string HelpString { get; set; } = "WinDebug connection string format: 'windebug'";

        private bool IsWinDebugConnectionString(string connectionString)
        {
            return connectionString.StartsWith("windebug");
        }
    }
}