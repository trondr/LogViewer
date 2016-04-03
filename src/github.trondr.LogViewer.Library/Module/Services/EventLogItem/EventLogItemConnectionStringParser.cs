using System;
using System.Text.RegularExpressions;
using Common.Logging;

namespace github.trondr.LogViewer.Library.Module.Services.EventLogItem
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
            if(match.Success)
            {
                var logName = match.Groups[1].Value;
                var machine = match.Groups[2].Value;
                var source = match.Groups[3].Value;
                var eventLogConnection = _eventLogItemConnectionFactory.GetEventLogItemConnection(connectionString, logName, machine, source);
                return eventLogConnection;    
            }
            _logger.WarnFormat("Invalid connection string '{0}'. Eventlog connection string must be on the format: 'eventlog:<log name>:<machine>:<source>'", connectionString);
            return null;            
        }
    }
}