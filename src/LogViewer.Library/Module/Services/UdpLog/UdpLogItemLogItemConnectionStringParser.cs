using System;
using System.Net;
using System.Text.RegularExpressions;
using Common.Logging;
using LogViewer.Library.Module.Services.TcpLog;

namespace LogViewer.Library.Module.Services.UdpLog
{

    public class UdpLogItemLogItemConnectionStringParser : IUdpLogItemConnectionStringParser
    {
        private readonly IUdpLogItemConnectionFactory _udpLogItemConnectionFactory;
        private readonly ILog _logger;
        private readonly Regex _connectionRegex = new Regex(@"^udp:(.+?):(\d+):(Ipv4|Ipv6)(|:2(?:2[4-9]|3\d)(?:\.(?:25[0-5]|2[0-4]\d|1\d\d|[1-9]\d?|0)){3})$", RegexOptions.IgnoreCase);

        public UdpLogItemLogItemConnectionStringParser(IUdpLogItemConnectionFactory udpLogItemConnectionFactory, ILog logger)
        {
            _udpLogItemConnectionFactory = udpLogItemConnectionFactory;
            _logger = logger;
        }

        public bool CanParse(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentException("Argument is null or empty", nameof(connectionString));
            var isMatch = _connectionRegex.IsMatch(connectionString);
            return isMatch;
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
                var multicastAddressString = match.Groups[4].Value.Replace(":","");
                ILogItemConnection connection;
                if (!string.IsNullOrWhiteSpace(multicastAddressString))
                {
                    var multicastAddress = IPAddress.Parse(multicastAddressString);
                    connection = _udpLogItemConnectionFactory.GetUdpLogItemConnection(connectionString, hostName, port, ipVersion, multicastAddress);
                }
                else
                {
                    connection = _udpLogItemConnectionFactory.GetUdpLogItemConnection(connectionString, hostName, port, ipVersion); 
                }                
                return connection;    
            }
            var message = string.Format("Invalid udp connection string '{0}'. Valid {1}", connectionString, HelpString);
            throw new InvalidConnectionStringException(message);
        }

        public string HelpString { get; set; } = "Udp connection string format: 'udp:<hostname>:<port>:<Ipv4|Ipv6>[:<multi cast ip address (between 224.0.0.1 and 239.255.255.255)>]'";
    }
}