using System;
using System.Text.RegularExpressions;
using Common.Logging;

namespace github.trondr.LogViewer.Library.Module.Services.TcpLog
{

    public class TcpLogItemLogItemConnectionStringParser : ITcpLogItemConnectionStringParser
    {
        private readonly ITcpLogItemConnectionFactory _tcpLogItemConnectionFactory;
        private readonly ILog _logger;
        private readonly Regex _connectionRegex = new Regex(@"^tcp:(.+?):(\d+):(Ipv4|Ipv6)$", RegexOptions.IgnoreCase);

        public TcpLogItemLogItemConnectionStringParser(ITcpLogItemConnectionFactory tcpLogItemConnectionFactory, ILog logger)
        {
            _tcpLogItemConnectionFactory = tcpLogItemConnectionFactory;
            _logger = logger;
        }

        public bool CanParse(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentException("Argument is null or empty", nameof(connectionString));
            return _connectionRegex.IsMatch(connectionString);
        }

        public ILogItemConnection Parse(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentException("Argument is null or empty", nameof(connectionString));
            var match = _connectionRegex.Match(connectionString);
            if(match.Success)
            {
                var hostName = match.Groups[1].Value;
                var port = Convert.ToInt32(match.Groups[2].Value);
                var ipVersion = (IpVersion)Enum.Parse(typeof(IpVersion), match.Groups[3].Value, true);
                var connection = _tcpLogItemConnectionFactory.GetTcpLogItemConnection(connectionString, hostName, port, ipVersion);
                return connection;    
            }
            _logger.WarnFormat("Invalid connection string '{0}'. Tcp connection string must be on the format: 'tcp:<hostname>:<port>:<Ipv4|Ipv6>'", connectionString);
            return null;
        }
    }
}