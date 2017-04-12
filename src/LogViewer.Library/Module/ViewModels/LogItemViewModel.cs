using System;
using GalaSoft.MvvmLight;
using LogViewer.Library.Module.Common.Collection;
using LogViewer.Library.Module.Common.UI;
using LogViewer.Library.Module.Services;

namespace LogViewer.Library.Module.ViewModels
{
    
    public class LogItemViewModel : ViewModelBase, ILogItemViewModel
    {
        private string _exceptionString;
        private bool _isVisible;
        private LoggerViewModel _logger;
        private LogLevelViewModel _logLevel;
        private string _message;
        private ObservableDictionary<string, string> _properties;
        private string _threadId;
        private DateTime _time;
        private SourceCodeInfo _sourceCode;
        private uint _sourceCodeLine = 1;
        private string _sourceCodeDetails;

        public string ExceptionString
        {
            get { return _exceptionString; }
            set { this.SetProperty(ref _exceptionString, value); }
        }

        public bool IsVisible
        {
            get { return _isVisible; }
            set { this.SetProperty(ref _isVisible, value); }
        }

        public LoggerViewModel Logger
        {
            get { return _logger; }
            set { this.SetProperty(ref _logger, value); }
        }

        public LogLevelViewModel LogLevel
        {
            get { return _logLevel; }
            set { this.SetProperty(ref _logLevel, value); }
        }

        public string Message
        {
            get { return _message; }
            set { this.SetProperty(ref _message, value); }
        }

        public ObservableDictionary<string, string> Properties
        {
            get { return _properties; }
            set { this.SetProperty(ref _properties, value); }
        }

        public string ThreadId
        {
            get { return _threadId; }
            set { this.SetProperty(ref _threadId, value); }
        }

        public DateTime Time
        {
            get { return _time; }
            set { this.SetProperty(ref _time, value); }
        }

        public SourceCodeInfo SourceCode
        {
            get { return _sourceCode; }
            set { this.SetProperty(ref _sourceCode, value); }
        }

        public uint SourceCodeLine
        {
            get { return _sourceCodeLine; }
            set { this.SetProperty(ref _sourceCodeLine, value); }
        }

        public string SourceCodeDetails
        {
            get { return _sourceCodeDetails; }
            set { this.SetProperty(ref _sourceCodeDetails, value); }
        }
    }
}