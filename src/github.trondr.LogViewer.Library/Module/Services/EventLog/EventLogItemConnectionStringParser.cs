using System;
using System.Text.RegularExpressions;
using Common.Logging;
using github.trondr.LogViewer.Library.Module.Services.UdpLog;

namespace github.trondr.LogViewer.Library.Module.Services.EventLog
{
    public class EventLogItemConnectionStringParser : IEventLogItemConnectionStringParser
    {
        private readonly IEventLogItemConnectionFactory _eventLogItemConnectionFactory;
        private readonly ILog _logger;
        private readonly Regex _eventLogConnectionRegex = new Regex("^eventlog:(.+?):(.+?):(.+?)$", RegexOptions.IgnoreCase);

        public EventLogItemConnectionStringParser(IEventLogItemConnectionFactory eventLogItemConnectionFactory, ILog logger)
        {
            _eventLogItemConnectionFactory = eventLogItemConnectionFactory;
            _logger = logger;
        }

        public bool CanParse(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentException("Argument is null or empty", nameof(connectionString));
            return _eventLogConnectionRegex.IsMatch(connectionString);
        }

        public ILogItemConnection Parse(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentException("Argument is null or empty", nameof(connectionString));
            var match = _eventLogConnectionRegex.Match(connectionString);
            if (match.Success)
            {
                var logName = match.Groups[1].Value;
                var machine = match.Groups[2].Value;
                var source = match.Groups[3].Value;
                var eventLogConnection = _eventLogItemConnectionFactory.GetEventLogItemConnection(connectionString, logName, machine, source);
                return eventLogConnection;
            }
            var message = string.Format("Invalid eventlog connection string '{0}'. Valid {1}", connectionString, HelpString);
            throw new InvalidConnectionStringException(message);
        }

        public string HelpString { get; set; } = @"Eventlog connection string format: 'eventlog:<log name>:<machine>:<source>'";
    }
}