using System.Collections.Generic;

namespace github.trondr.LogViewer.Library.Services
{
    public class LogItemConnectionProvider : ILogItemConnectionProvider
    {
        private readonly IEnumerable<ILogItemConnectionStringParser> _connectionStringParsers;


        public LogItemConnectionProvider(IEnumerable<ILogItemConnectionStringParser> connectionStringParsers)
        {
            _connectionStringParsers = connectionStringParsers;
        }


        public IEnumerable<ILogItemConnection> GetLogItemConnections(string[] connectionStrings)
        {
            foreach (var connectionString in connectionStrings)
            {
                foreach (var logItemConnectionStringParser in _connectionStringParsers)
                {
                    if(logItemConnectionStringParser.CanParse(connectionString))
                    {
                        yield return logItemConnectionStringParser.Parse(connectionString);
                    }
                }
            }            
        }
    }
}