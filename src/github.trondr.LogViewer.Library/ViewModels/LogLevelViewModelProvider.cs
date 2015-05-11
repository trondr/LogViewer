using System;
using System.Collections.Generic;
using github.trondr.LogViewer.Library.Infrastructure;

namespace github.trondr.LogViewer.Library.ViewModels
{
    [Singleton]
    public class LogLevelViewModelProvider : ILogLevelViewModelProvider
    {
        private readonly Dictionary<string, LogLevelViewModel> _logLevels;

        public LogLevelViewModelProvider()
        {
            _logLevels = new Dictionary<string, LogLevelViewModel>();
        }

        public LogLevelViewModel GetLevel(string logLevel)
        {
            if(_logLevels.ContainsKey(logLevel))
            {
                return  _logLevels[logLevel];
            }
            var logLevelValue = (LogLevel)Enum.Parse(typeof(LogLevel), logLevel);
            var logLevelViewModel = new LogLevelViewModel { Level = logLevelValue };
            _logLevels.Add(logLevel, logLevelViewModel);
            return logLevelViewModel;
        }
    }
}