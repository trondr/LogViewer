using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Common.Logging;
using github.trondr.LogViewer.Library.Model;

namespace github.trondr.LogViewer.Library.Services
{
    /// <summary>
    /// Credits: http://log2console.codeplex.com/SourceControl/latest#src/Log2Console/Receiver/FileReceiver.cs
    /// </summary>
    public class FileLogItemReceiver : IFileLogItemReceiver
    {
        private readonly ILog4JLogItemParser _log4JLogItemParser;
        private readonly ILog _logger;
        private ILogItemNotifiable _logItemNotifiable = null;
        private long _lastFileLength;
        private FileSystemWatcher _fileSystemWatcher;
        private string _fullLoggerName;
        private string _logFileName;

        public FileLogItemReceiver(ILog4JLogItemParser log4JLogItemParser, ILog logger)
        {
            _log4JLogItemParser = log4JLogItemParser;
            _logger = logger;
        }

        public void Initialize()
        {
            if(string.IsNullOrEmpty(LogFileName))
            {
                _logger.Warn("Log file name is null or empty. Initialization of FileLogItemReceiver cannot continue.");
                return;
            }
            if(File.Exists(LogFileName))
            {
                using (var fileStream = new FileStream(LogFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    using(var sr = new StreamReader(fileStream))
                    {
                        _lastFileLength = ShowFromBegining ? 0 : sr.BaseStream.Length;
                    }
                }
            }
            var fullFileName = Path.GetFullPath(LogFileName);
            var folder = Path.GetDirectoryName(fullFileName);
            var file = Path.GetFileName(fullFileName);
            if (folder != null) 
            {
                _fileSystemWatcher = new FileSystemWatcher(folder,file);
                _fileSystemWatcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.Size;
                _fileSystemWatcher.Changed+=OnFileChanged;
                _fileSystemWatcher.Deleted+=OnFileDeleted;
                _fileSystemWatcher.Renamed+=OnFileRenamed;     
                _fileSystemWatcher.EnableRaisingEvents = true;
            }            
            ComputeFullLoggerName();
        }

        public void Terminate()
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

        public void Attach(ILogItemNotifiable logItemNotifiable)
        {
            _logItemNotifiable = logItemNotifiable;
        }

        public void Detach()
        {
            _logItemNotifiable = null;
        }

        public string DefaultLoggerName { get; set; }

        public string LogFileName
        {
            get { return _logFileName; }
            set
            {
                if(String.Compare(_logFileName, value, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    return;
                }                
                _logFileName = value;
                Terminate();
                Initialize();
            }
        }

        public bool ShowFromBegining { get; set; }

        private void ReadFile()
        {
            if (!File.Exists(LogFileName))
            {
                _logger.WarnFormat("Cannot read log file '{0}'. Log file does not exist.", LogFileName);
                return;
            }

            using (var fileStream = new FileStream(LogFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (var sr = new StreamReader(fileStream))
                {
                    if (sr.BaseStream.Length == _lastFileLength)
                        return;

                    if (sr.BaseStream.Length < _lastFileLength)
                    {
                        //Log file has been trucated/deleted/renamed
                        _lastFileLength = 0;
                    }

                    // Seek to the last file length
                    sr.BaseStream.Seek(_lastFileLength, SeekOrigin.Begin);

                    // Get last added lines
                    string line;
                    var logItemString = new StringBuilder();
                    var logItems = new List<LogItem>();
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

                    // Notify the UI with the set of messages
                    _logItemNotifiable.Notify(logItems.ToArray());

                    // Update the last file length
                    _lastFileLength = sr.BaseStream.Position;
                }
            }
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
            _fullLoggerName = String.Format("FileLogger.{0}",
                String.IsNullOrEmpty(DefaultLoggerName)
                    ? LogFileName.Replace('.', '_')
                    : DefaultLoggerName);

        }
    }
}