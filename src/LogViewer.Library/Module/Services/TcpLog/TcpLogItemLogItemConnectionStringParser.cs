using System;
using System.Text.RegularExpressions;

namespace LogViewer.Library.Module.Services.TcpLog
{

    public class TcpLogItemLogItemConnectionStringParser : ITcpLogItemConnectionStringParser
    {
        private readonly ITcpLogItemConnectionFactory _tcpLogItemConnectionFactory;
        private readonly Regex _connectionRegex = new Regex(@"^tcp:(.+?):(\d+):(Ipv4|Ipv6)$", RegexOptions.IgnoreCase);

        public TcpLogItemLogItemConnectionStringParser(ITcpLogItemConnectionFactory tcpLogItemConnectionFactory)
        {
            _tcpLogItemConnectionFactory = tcpLogItemConnectionFactory;
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
            var message = string.Format("Invalid tcp connection string '{0}'. Valid {1}", connectionString, HelpString);
            throw new InvalidConnectionStringException(message);
        }

        public string HelpString { get; set; } = "Tcp connection string format: 'tcp:<hostname>:<port>:<Ipv4|Ipv6>'";
    }
}