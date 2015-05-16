using System;
using System.Collections.Generic;
using github.trondr.LogViewer.Library.Common.UI;
using github.trondr.LogViewer.Library.ViewModels;

namespace github.trondr.LogViewer.Library.Services
{
    public class TestDataLogItemService : ILogItemService
    {
        private readonly ILoggerViewModelProvider _loggerViewModelProvider;
        private readonly ILogLevelViewModelProvider _logLevelViewModelProvider;

        public TestDataLogItemService(ILoggerViewModelProvider loggerViewModelProvider, ILogLevelViewModelProvider logLevelViewModelProvider)
        {
            _loggerViewModelProvider = loggerViewModelProvider;
            _logLevelViewModelProvider = logLevelViewModelProvider;
        }

        public IEnumerable<LogItemViewModel> GetLogs()
        {
            Console.Write("+");
            var logger1 = _loggerViewModelProvider.GetLogger("Company.Product.Class1");
            var logger2 = _loggerViewModelProvider.GetLogger("Company.Product.Class2");
            var logger3 = _loggerViewModelProvider.GetLogger("Company.Product.Class3");
            var logger4 = _loggerViewModelProvider.GetLogger("Company.Product.Class4");
            var traceLogLevel = _logLevelViewModelProvider.GetLevel("Trace");
            var debugLogLevel = _logLevelViewModelProvider.GetLevel("Debug");
            var infoLogLevel = _logLevelViewModelProvider.GetLevel("Info");
            var warnLogLevel = _logLevelViewModelProvider.GetLevel("Warn");
            var errorLogLevel = _logLevelViewModelProvider.GetLevel("Error");
            var fatalLogLevel = _logLevelViewModelProvider.GetLevel("Fatal");
            for (var i = 0; i < 100; i++)
            {
                yield return new LogItemViewModel { Time = DateTime.Now.AddSeconds(-31), LogLevel = errorLogLevel, Logger = logger1, ThreadId = 1, Message = "Some error message" };
                yield return new LogItemViewModel { Time = DateTime.Now.AddSeconds(-30), LogLevel = warnLogLevel, Logger = logger2, ThreadId = 1, Message = "Some warn message" };
                yield return new LogItemViewModel { Time = DateTime.Now.AddSeconds(-29), LogLevel = infoLogLevel, Logger = logger3, ThreadId = 1, Message = "Some info message" };
                yield return new LogItemViewModel { Time = DateTime.Now.AddSeconds(-28), LogLevel = debugLogLevel, Logger = logger4, ThreadId = 1, Message = "Some debug message" };
                yield return new LogItemViewModel { Time = DateTime.Now.AddSeconds(-27), LogLevel = traceLogLevel, Logger = logger1, ThreadId = 1, Message = "Some trace message" };
                yield return new LogItemViewModel { Time = DateTime.Now.AddSeconds(-26), LogLevel = fatalLogLevel, Logger = logger2, ThreadId = 1, Message = "Some fatal message" };
                yield return new LogItemViewModel { Time = DateTime.Now.AddSeconds(-25), LogLevel = errorLogLevel, Logger = logger3, ThreadId = 1, Message = "Some error message" };
                yield return new LogItemViewModel { Time = DateTime.Now.AddSeconds(-24), LogLevel = warnLogLevel, Logger = logger4, ThreadId = 1, Message = "Some warn  message" };
                yield return new LogItemViewModel { Time = DateTime.Now.AddSeconds(-23), LogLevel = infoLogLevel, Logger = logger1, ThreadId = 1, Message = "Some info  message" };
                yield return new LogItemViewModel { Time = DateTime.Now.AddSeconds(-22), LogLevel = debugLogLevel, Logger = logger2, ThreadId = 1, Message = "Some debug message" };
                yield return new LogItemViewModel { Time = DateTime.Now.AddSeconds(-21), LogLevel = traceLogLevel, Logger = logger3, ThreadId = 1, Message = "Some trace message" };
                yield return new LogItemViewModel { Time = DateTime.Now.AddSeconds(-20), LogLevel = fatalLogLevel, Logger = logger4, ThreadId = 1, Message = "Some fatal message" };
                yield return new LogItemViewModel { Time = DateTime.Now.AddSeconds(-19), LogLevel = errorLogLevel, Logger = logger1, ThreadId = 1, Message = "Some error message" };
                yield return new LogItemViewModel { Time = DateTime.Now.AddSeconds(-18), LogLevel = warnLogLevel, Logger = logger2, ThreadId = 1, Message = "Some warn  message" };
                yield return new LogItemViewModel { Time = DateTime.Now.AddSeconds(-17), LogLevel = infoLogLevel, Logger = logger3, ThreadId = 1, Message = "Some info  message" };
                yield return new LogItemViewModel { Time = DateTime.Now.AddSeconds(-16), LogLevel = debugLogLevel, Logger = logger4, ThreadId = 1, Message = "Some debug message" };    
            }            
        }                                                                                                                                                
    }
}