﻿using System.Collections.Generic;
using System.Linq;
using github.trondr.LogViewer.Library.Infrastructure;

namespace github.trondr.LogViewer.Library.ViewModels
{
    [Singleton]
    public class LoggerViewModelProvider: ILoggerViewModelProvider
    {
        private readonly Dictionary<string, LoggerViewModel> _loggers;
        private LoggerViewModel _root;
        private static readonly object Synch = new object();


        public LoggerViewModelProvider()
        {
            _loggers = new Dictionary<string, LoggerViewModel>();            
        }

        public LoggerViewModel GetLogger(string logger)
        {
            lock (Synch)
            {
                if (_loggers.ContainsKey(logger))
                {
                    return _loggers[logger];
                }
                var loggerViewModel = new LoggerViewModel(logger);
                _loggers.Add(logger, loggerViewModel);
                var parentLogger = GetParent(logger);
                if (parentLogger != null)
                {
                    loggerViewModel.Parent = GetLogger(parentLogger);
                    if (loggerViewModel.Parent.Children.All(model => model.Name != loggerViewModel.Name))
                    {
                        loggerViewModel.Parent.Children.Add(loggerViewModel);
                    }
                }
                else
                {
                    loggerViewModel.Parent = Root;
                    if (Root.Children.All(model => model.Name != loggerViewModel.Name))
                    {
                        Root.Children.Add(loggerViewModel);
                    }
                }
                return loggerViewModel;
            }
        }

        public LoggerViewModel Root
        {
            get
            {
                if(_root == null)
                {
                    _root = new LoggerViewModel("root");
                    _loggers.Add(_root.Name, _root);
                }
                return _root;
            }
            private set { _root = value; }
        }

        private string GetParent(string logger)
        {
            if(logger.Contains("."))
            {
                return logger.Substring(0, logger.LastIndexOf("."));
            }
            return null;
        }
    }
}