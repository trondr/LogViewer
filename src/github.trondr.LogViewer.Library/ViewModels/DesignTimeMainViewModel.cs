using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Media;
using github.trondr.LogViewer.Library.Common.UI;

namespace github.trondr.LogViewer.Library.ViewModels
{
    public class DesignTimeMainViewModel : ViewModelBase, IMainViewModel
    {
        public DesignTimeMainViewModel()
        {
            var loggerViewModelProvider = new LoggerViewModelProvider();
            var logger1 = loggerViewModelProvider.GetLogger("Company.Product.Class1");
            var logger2 = loggerViewModelProvider.GetLogger("Company.Product.Class2");
            var logger3 = loggerViewModelProvider.GetLogger("Company.Product.Class3");
            var logger4 = loggerViewModelProvider.GetLogger("Company.Product.Class4");

            var logLevelViewModelProvider = new LogLevelViewModelProvider();
            var traceLogLevel = logLevelViewModelProvider.GetLevel("Trace");
            var debugLogLevel = logLevelViewModelProvider.GetLevel("Debug");
            var infoLogLevel = logLevelViewModelProvider.GetLevel("Info");
            var warnLogLevel = logLevelViewModelProvider.GetLevel("Warn");
            var errorLogLevel = logLevelViewModelProvider.GetLevel("Error");
            var fatalLogLevel = logLevelViewModelProvider.GetLevel("Fatal");
            
            LogItems = new ObservableCollection<LogItemViewModel>();
            LogItems.Add(new LogItemViewModel(){Time = DateTime.Now.AddSeconds(-31),Level = fatalLogLevel, Logger = logger1,ThreadId = 1, Message = "Some fatal error message"});
            LogItems.Add(new LogItemViewModel(){Time = DateTime.Now.AddSeconds(-31),Level = errorLogLevel, Logger = logger2,ThreadId = 1, Message = "Some error message"});
            LogItems.Add(new LogItemViewModel(){Time = DateTime.Now.AddSeconds(-31),Level = warnLogLevel, Logger = logger3,ThreadId = 1, Message = "Some warning message"});
            LogItems.Add(new LogItemViewModel(){Time = DateTime.Now.AddSeconds(-31),Level = infoLogLevel, Logger = logger4,ThreadId = 1, Message = "Some info message"});
            LogItems.Add(new LogItemViewModel(){Time = DateTime.Now.AddSeconds(-31),Level = traceLogLevel, Logger = logger4,ThreadId = 1, Message = "Some trace message"});
            LogItems.Add(new LogItemViewModel(){Time = DateTime.Now.AddSeconds(-31),Level = debugLogLevel, Logger = logger4,ThreadId = 1, Message = "Some debug message"});
            Loggers = new ObservableCollection<LoggerViewModel> {loggerViewModelProvider.Root};

            LogLevels = new ObservableCollection<LogLevelViewModel>() { traceLogLevel, debugLogLevel, infoLogLevel, warnLogLevel, errorLogLevel, fatalLogLevel };
            
        }
        public ObservableCollection<LogItemViewModel> LogItems { get; set; }
        public ObservableCollection<LoggerViewModel> Loggers { get; set; }
        public ObservableCollection<LogLevelViewModel> LogLevels { get; set; }
        public ICommand ExitCommand { get; set; }

    }
}