using System;
using System.Text.RegularExpressions;
using Common.Logging;

namespace github.trondr.LogViewer.Library.Module.Services.FileLog
{

    public class FileLogItemConnectionStringParser : IFileLogItemConnectionStringParser
    {
        private readonly IFileLogItemConnectionFactory _fileLogItemConnectionFactory;
        private readonly ILog _logger;

        private readonly Regex _fileUrlRegEx = new Regex("^file://(.+)$");
        private readonly Regex _uncPathRegEx = new Regex(@"^(\\\\.+)$");
        private readonly Regex _localPathRegEx = new Regex(@"^([a-zA-z]:.+)$");

        public FileLogItemConnectionStringParser(IFileLogItemConnectionFactory fileLogItemConnectionFactory, ILog logger)
        {
            _fileLogItemConnectionFactory = fileLogItemConnectionFactory;
            _logger = logger;
        }

        public bool CanParse(string connectionString)
        {
            return IsFileUrl(connectionString) || IsUncPath(connectionString) || IsLocalPath(connectionString);
        }

        public ILogItemConnection Parse(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString)) throw new ArgumentException("Argument is null or empty", nameof(connectionString));

            var fileName = GetFileNameFromConnectionString(connectionString);

            if(!string.IsNullOrEmpty(fileName))
            {
                return _fileLogItemConnectionFactory.GetFileLogItemConnection(connectionString, fileName);
            }

            _logger.WarnFormat("Invalid connection string '{0}'. File log connection string must be on the format: 'file:\\<log file path>' or '<unc path>' or '<local path>'", connectionString);
            return null;
        }

        private string GetFileNameFromConnectionString(string connectionString)
        {
            var match = _fileUrlRegEx.Match(connectionString);
            if(match.Success)
            {
                var fileName = match.Groups[1].Value;
                return fileName;
            }
            match = _uncPathRegEx.Match(connectionString);
            if(match.Success)
            {
                var fileName = match.Groups[1].Value;
                return fileName;
            }
            match = _localPathRegEx.Match(connectionString);
            if(match.Success)
            {
                var fileName = match.Groups[1].Value;
                return fileName;
            }
            _logger.WarnFormat("Invalid connection string '{0}'. File log connection string must be on the format: 'file:\\<log file path>' or <unc path> or <local path>", connectionString);
            return null;
        }

        private bool IsFileUrl(string connectionString)
        {
            return _fileUrlRegEx.IsMatch(connectionString);
        }

        private bool IsUncPath(string connectionString)
        {
            return _uncPathRegEx.IsMatch(connectionString);
        }

        private bool IsLocalPath(string connectionString)
        {
            return _localPathRegEx.IsMatch(connectionString);
        }
    }
}