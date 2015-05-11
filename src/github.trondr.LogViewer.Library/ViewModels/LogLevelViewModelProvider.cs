using System;
using System.Collections.Generic;

namespace github.trondr.LogViewer.Library.ViewModels
{
    public class LogLevelViewModelProvider : ILogLevelViewModelProvider
    {
        private readonly Dictionary<string, LogLevelViewModel> _loglevel;

        public LogLevelViewModelProvider()
        {
            _loglevel = new Dictionary<string, LogLevelViewModel>();
        }

        public LogLevelViewModel GetLevel(string logLevel)
        {
            if(_loglevel.ContainsKey(logLevel))
            {
                return  _loglevel[logLevel];
            }
            var logLevelValue = (LogLevel)Enum.Parse(typeof(LogLevel), logLevel);
            var logLevelViewModel = new LogLevelViewModel { Level = logLevelValue };
            _loglevel.Add(logLevel, logLevelViewModel);
            return logLevelViewModel;
        }
    }
}