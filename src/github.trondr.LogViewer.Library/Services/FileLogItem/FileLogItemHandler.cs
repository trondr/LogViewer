using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Common.Logging;
using github.trondr.LogViewer.Library.Model;

namespace github.trondr.LogViewer.Library.Services.FileLogItem
{
    public class FileLogItemHandler : ILogItemHandler<FileLogItemConnection>
    {
        private readonly ILog4JLogItemParser _log4JLogItemParser;
        private readonly IFileLogItemConnectionStringParser _fileLogItemConnectionStringParser;
        private readonly ILog _logger;
        private ILogItemNotifiable _logItemNotifiable = null;
        private long _lastFileLength;
        private FileSystemWatcher _fileSystemWatcher;
        private string _fullLoggerName;        
        private bool _showFromBeginning;
        private ILogItemConnection _connection;
        private string _logFileName;

        public FileLogItemHandler(ILog4JLogItemParser log4JLogItemParser, IFileLogItemConnectionStringParser fileLogItemConnectionStringParser, ILog logger)
        {
            _log4JLogItemParser = log4JLogItemParser;
            _fileLogItemConnectionStringParser = fileLogItemConnectionStringParser;
            _logger = logger;
        }

        public ILogItemConnection Connection
        {
            get { return _connection; }
            set
            {
                if ((_connection != null) && string.Compare(_connection.Value, value.Value, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    return;
                }
                _connection = value;
                _logFileName = _connection?.Value?.Replace("file://", "");
                //Terminate();
                //Initialize();
            }
        }

        public void Initialize()
        {
            if (Connection == null)
            {
                _logger.Warn("Connection info has not been initialized. Initialization of cannot continue.");
                return;
            }

            if (string.IsNullOrEmpty(_logFileName))
            {
                _logger.Warn("Log file name is null or empty. Initialization cannot continue.");
                return;
            }
            ComputeFullLoggerName();
            StartMonitoringLogFile(_logFileName);
        }

        public void Terminate()
        {
            StopMonitoringLogFile();
        }

        public void Attach(ILogItemNotifiable logItemNotifiable)
        {
            _logItemNotifiable = logItemNotifiable;
            if (ShowFromBeginning)
                ReadFile();
        }

        public void Detach()
        {
            _logItemNotifiable = null;
        }

        private void StartMonitoringLogFile(string logFileName)
        {
            _lastFileLength = CalculateLastFileLength();
            var file = Path.GetFileName(logFileName);
            if (file == null) return;

            var folder = Path.GetDirectoryName(logFileName);
            if (folder == null) return;

            _fileSystemWatcher = new FileSystemWatcher(folder, file)
            {
                NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.Size
            };
            _fileSystemWatcher.Changed += OnFileChanged;
            _fileSystemWatcher.Deleted += OnFileDeleted;
            _fileSystemWatcher.Renamed += OnFileRenamed;
            _fileSystemWatcher.EnableRaisingEvents = true;
        }

        private long CalculateLastFileLength()
        {
            if (File.Exists(_logFileName))
            {
                using (var fileStream = new FileStream(_logFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    using (var sr = new StreamReader(fileStream))
                    {
                        return ShowFromBeginning ? 0 : sr.BaseStream.Length;
                    }
                }
            }
            return 0;
        }

        private void StopMonitoringLogFile()
        {
            if (_fileSystemWatcher != null)
            {
                _fileSystemWatcher.EnableRaisingEvents = false;
                _fileSystemWatcher.Changed -= OnFileChanged;
                _fileSystemWatcher.Deleted -= OnFileDeleted;
                _fileSystemWatcher.Renamed -= OnFileRenamed;
                _fileSystemWatcher.Dispose();
                _fileSystemWatcher = null;
            }
            _lastFileLength = 0;
        }

        public string DefaultLoggerName { get; set; }

        public bool ShowFromBeginning
        {
            get { return _showFromBeginning; }
            set
            {
                _showFromBeginning = value;
                if (_showFromBeginning && _lastFileLength == 0)
                {
                    ReadFile();
                }
            }
        }

        private void ReadFile()
        {
            if (!File.Exists(_logFileName))
            {
                _logger.WarnFormat("Cannot read log file '{0}'. Log file does not exist.", _logFileName);
                return;
            }

            using (var fileStream = new FileStream(_logFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (var sr = new StreamReader(fileStream))
                {
                    if (sr.BaseStream.Length == _lastFileLength)
                        //No new entries
                        return;

                    if (sr.BaseStream.Length < _lastFileLength)
                    {
                        //Log file has been trucated/deleted/renamed
                        _lastFileLength = 0;
                    }

                    var logItems = GetLastAddedLogItems(sr, _lastFileLength);

                    if (NotifyUserInterfaceAboutNewLogItems(logItems))
                    {
                        // Update the last file length
                        _lastFileLength = sr.BaseStream.Position;
                    }
                }
            }
        }

        private bool NotifyUserInterfaceAboutNewLogItems(List<LogItem> logItems)
        {
            if (_logItemNotifiable != null)
            {
                _logItemNotifiable.Notify(logItems.ToArray());
                return true;
            }
            return false;
        }

        private List<LogItem> GetLastAddedLogItems(StreamReader sr, long lastFileLength)
        {
            // Seek to the last file length
            sr.BaseStream.Seek(lastFileLength, SeekOrigin.Begin);
            var logItems = new List<LogItem>();
            var logItemString = new StringBuilder();
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                logItemString.Append(line);
                // This condition allows us to process events that spread over multiple lines
                if (line.Contains("</log4j:event>"))
                {
                    var logMsg = _log4JLogItemParser.Parse(logItemString.ToString(), _fullLoggerName);
                    logItems.Add(logMsg);
                    logItemString.Clear();
                    logItemString = new StringBuilder();
                }
            }
            return logItems;
        }

        void OnFileRenamed(object sender, RenamedEventArgs e)
        {
            if (e.ChangeType != WatcherChangeTypes.Renamed)
                return;

            Terminate();
            Initialize();
        }

        void OnFileDeleted(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType != WatcherChangeTypes.Deleted)
                return;

            Terminate();
            Initialize();
        }

        void OnFileChanged(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType != WatcherChangeTypes.Changed)
                return;

            ReadFile();
        }

        private void ComputeFullLoggerName()
        {
            var fileName = Path.GetFileName(_logFileName);
            _fullLoggerName = string.Format("FileLogger.{0}",
                string.IsNullOrEmpty(DefaultLoggerName) && !string.IsNullOrEmpty(fileName)
                    ? fileName.Replace('.', '_')
                    : DefaultLoggerName);

        }
    }
}