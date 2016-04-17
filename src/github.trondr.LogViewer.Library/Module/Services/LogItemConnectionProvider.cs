using System;
using System.Collections.Generic;
using System.Text;

namespace github.trondr.LogViewer.Library.Module.Services
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
            var connectionStringIsSupportedDictionary = new Dictionary<string, object>();
            foreach (var connectionString in connectionStrings)
            {
                foreach (var logItemConnectionStringParser in _connectionStringParsers)
                {
                    if(logItemConnectionStringParser.CanParse(connectionString))
                    {
                        AddConnectionStringToIsSupportedDictionary(connectionString, connectionStringIsSupportedDictionary);
                        yield return logItemConnectionStringParser.Parse(connectionString);
                    }
                }
            }
            VerifyThatAllConnectionStringsIsSupported(connectionStrings, connectionStringIsSupportedDictionary);
        }

        private void VerifyThatAllConnectionStringsIsSupported(string[] connectionStrings, Dictionary<string, object> supportedDictionary)
        {            
            var unsupportedConnectionStrings = new List<string>();
            foreach (var connectionString in connectionStrings)
            {
                if(!supportedDictionary.ContainsKey(connectionString))
                {
                    unsupportedConnectionStrings.Add(connectionString);
                }
                
            }
            if(unsupportedConnectionStrings.Count > 0)
            {
                var sb = new StringBuilder();
                sb.Append("Connection string(s) not supported: ");
                foreach (var unsupportedConnectionString in unsupportedConnectionStrings)
                {
                    sb.AppendFormat("'{0}', ", unsupportedConnectionString);
                }
                throw new NotSupportedException(sb.ToString().TrimEnd(',', ' '));
            }            
        }

        private void AddConnectionStringToIsSupportedDictionary(string connectionString, Dictionary<string, object> supportedDictionary)
        {
            supportedDictionary.Add(connectionString,null);
        }
    }
}