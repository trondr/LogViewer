using System.Text.RegularExpressions;

namespace github.trondr.LogViewer.Library.Services.FileLogItem
{

   public class FileLogItemLogItemConnectionStringParser: IFileLogItemConnectionStringParser
    {
        private readonly IFileLogItemConnectionFactory _fileLogItemConnectionStringFactory;

        public FileLogItemLogItemConnectionStringParser(IFileLogItemConnectionFactory fileLogItemConnectionStringFactory)
        {
            _fileLogItemConnectionStringFactory = fileLogItemConnectionStringFactory;
        }

        public bool CanParse(string connectionString)
        {
            return IsFileUrl(connectionString) || IsUncPath(connectionString) || IsLocalPath(connectionString);
        }

        public ILogItemConnection Parse(string connectionString)
        {
            return _fileLogItemConnectionStringFactory.GetFileLogItemConnection(connectionString);
        }

        private bool IsFileUrl(string connectionString)
        {
            return connectionString.StartsWith("file://");
        }

        private bool IsUncPath(string connectionString)
        {
            return connectionString.StartsWith(@"\\");
        }

        private bool IsLocalPath(string connectionString)
        {
            return Regex.IsMatch(connectionString, "^[a-zA-z]:");
        }
    }
}