using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Timers;
using Common.Logging;
using LogViewer.Library.Module.Model;

namespace LogViewer.Library.Module.Services.FileLog
{
    public class FileLogItemHandler : ILogItemHandler<FileLogItemConnection>
    {
        private readonly ILog4JLogItemParser _log4JLogItemParser;        
        private readonly ILog _logger;
        private ILogItemNotifiable _logItemNotifiable;
        private long _lastFileLength;
        private FileSystemWatcher _fileSystemWatcher;

        private string _defaultLoggerName;
        private Timer _timer;

        public FileLogItemHandler(ILog4JLogItemParser log4JLogItemParser, ILog logger)
        {
            _log4JLogItemParser = log4JLogItemParser;            
            _logger = logger;
        }

        public ILogItemConnection Connection { get; set; }

        public void Initialize()
        {
            var connection = GetConnection();            
            StartMonitoringLogFile(connection.FileName);
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

            //Ref issue discussed in https://blogs.technet.microsoft.com/asiasupp/2010/12/14/file-date-modified-property-are-not-updating-while-modifying-a-file-without-closing-it/
            //Issue is effectively disabling flush to disk even if 'ImmediateFlush' is configured in log4net. It seems that checking for file existence is enough to trigger an actual flush.
            _timer = new Timer(500);            
            _timer.Elapsed += (sender, args) => { File.Exists(logFileName);  };
            _timer.Start();
        }

        private long CalculateLastFileLength()
        {
            var connection = GetConnection();
            var lastFileLength = ShowFromBeginning ? 0 : GetFileLength(connection.FileName);
            return lastFileLength;            
        }
        
        private long GetFileLength(string fileName)
        {
            long fileLength = 0;
            if (File.Exists(fileName))
            {
                using (var fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    using (var sr = new StreamReader(fileStream))
                    {
                        fileLength = sr.BaseStream.Length;
                    }
                }
            }
            return fileLength;
        }

        private void StopMonitoringLogFile()
        {
            _timer?.Stop();
            _timer?.Dispose();
            _timer = null;
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

        public string DefaultLoggerName
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_defaultLoggerName))
                {
                    var connection = GetConnection();
                    var fileName = Path.GetFileName(connection.FileName);
                    var underscoredFileName = fileName?.Replace('.', '_');
                    _defaultLoggerName = $"FileLogger.{underscoredFileName}";
                }
                return _defaultLoggerName;
            }
            set { _defaultLoggerName = value; }
        }

        public bool ShowFromBeginning { get; set; }

        private void ReadFile()
        {
            var connection = GetConnection();
            if (!File.Exists(connection.FileName))
            {
                _logger.WarnFormat("Cannot read log file '{0}'. Log file does not exist.", connection.FileName);
                return;
            }

            using (var fileStream = new FileStream(connection.FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
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
                    var logMsg = _log4JLogItemParser.Parse(logItemString.ToString(), DefaultLoggerName);
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
        
        IFileLogItemConnection GetConnection()
        {
            var connection = Connection as IFileLogItemConnection;
            ValidateConnection(connection);
            return connection;
        }

        private void ValidateConnection(IFileLogItemConnection eventLogConnection)
        {
            if (eventLogConnection == null) throw new ArgumentNullException(nameof(eventLogConnection));
            if (string.IsNullOrEmpty(eventLogConnection.FileName)) throw new LogViewerException("FileName has not been initialized.");            
        }
    }
}