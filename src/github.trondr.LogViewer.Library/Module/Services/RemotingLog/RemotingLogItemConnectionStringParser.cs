using System;
using System.Text.RegularExpressions;
using Common.Logging;

namespace github.trondr.LogViewer.Library.Module.Services.RemotingLog
{
    public class RemotingLogItemConnectionStringParser : IRemotingLogItemConnectionStringParser
    {
        private readonly IRemotingLogItemConnectionFactory _remotingLogItemConnectionFactory;
        private readonly ILog _logger;
        private readonly Regex _connectionStringRegex = new Regex(@"^remoting:(.+?):(\d+)$", RegexOptions.IgnoreCase);

        public RemotingLogItemConnectionStringParser(IRemotingLogItemConnectionFactory remotingLogItemConnectionFactory, ILog logger)
        {
            _remotingLogItemConnectionFactory = remotingLogItemConnectionFactory;
            _logger = logger;
        }

        public bool CanParse(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentException("Argument is null or empty", nameof(connectionString));
            return _connectionStringRegex.IsMatch(connectionString);
        }

        public ILogItemConnection Parse(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentException("Argument is null or empty", nameof(connectionString));
            var match = _connectionStringRegex.Match(connectionString);
            if (match.Success)
            {
                var sinkName = match.Groups[1].Value;
                var port = match.Groups[2].Value;                
                var connection = _remotingLogItemConnectionFactory.GetRemotingLogItemConnection(connectionString, sinkName, port);
                return connection;
            }
            var message = string.Format("Invalid remoting connection string '{0}'. Valid {1}", connectionString, HelpString);
            throw new InvalidConnectionStringException(message);
        }

        public string HelpString { get; set; } = @"Remoting connection string format: 'remoting:<sink name>:<port>'";
    }
}