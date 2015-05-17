using System;
using System.Collections.Generic;
using github.trondr.LogViewer.Library.Infrastructure;
using github.trondr.LogViewer.Library.Model;

namespace github.trondr.LogViewer.Library.ViewModels
{
    [Singleton]
    public class LogLevelViewModelProvider : ILogLevelViewModelProvider
    {
        private readonly ILogLevelSettings _logLevelSettings;
        private readonly Dictionary<string, LogLevelViewModel> _logLevels;
        private static readonly object Synch = new object();

        public LogLevelViewModelProvider(ILogLevelSettings logLevelSettings)
        {
            _logLevelSettings = logLevelSettings;
            _logLevels = new Dictionary<string, LogLevelViewModel>();
        }

        public LogLevelViewModel GetLevel(string logLevel)
        {
            lock (Synch)
            {
                if (_logLevels.ContainsKey(logLevel))
                {
                    return _logLevels[logLevel];
                }
                var logLevelValue = (LogLevel)Enum.Parse(typeof(LogLevel), logLevel);
                var logLevelViewModel = new LogLevelViewModel { Level = logLevelValue, Color = _logLevelSettings.LoadForegroundColor(logLevelValue)};
                _logLevels.Add(logLevel, logLevelViewModel);
                return logLevelViewModel;
            }
        }
    }
}