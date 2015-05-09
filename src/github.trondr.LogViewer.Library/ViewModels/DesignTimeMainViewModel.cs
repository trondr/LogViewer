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
            
            LogItems = new ObservableCollection<LogItemViewModel>();
            LogItems.Add(new LogItemViewModel(){Time = DateTime.Now.AddSeconds(-31),Level = new LogLevelViewModel(){Level = LogLevel.Fatal, Color = Colors.Red}, Logger = logger1,ThreadId = 1, Message = "Some fatal error message"});
            LogItems.Add(new LogItemViewModel(){Time = DateTime.Now.AddSeconds(-31),Level = new LogLevelViewModel(){Level = LogLevel.Error, Color = Colors.LightCoral}, Logger = logger2,ThreadId = 1, Message = "Some error message"});
            LogItems.Add(new LogItemViewModel(){Time = DateTime.Now.AddSeconds(-31),Level = new LogLevelViewModel(){Level = LogLevel.Warning, Color = Colors.Orange}, Logger = logger3,ThreadId = 1, Message = "Some warning message"});
            LogItems.Add(new LogItemViewModel(){Time = DateTime.Now.AddSeconds(-31),Level = new LogLevelViewModel(){Level = LogLevel.Info, Color = Colors.DarkSeaGreen}, Logger = logger4,ThreadId = 1, Message = "Some info message"});
            LogItems.Add(new LogItemViewModel(){Time = DateTime.Now.AddSeconds(-31),Level = new LogLevelViewModel(){Level = LogLevel.Trace, Color = Colors.DarkGray}, Logger = logger4,ThreadId = 1, Message = "Some trace message"});
            LogItems.Add(new LogItemViewModel(){Time = DateTime.Now.AddSeconds(-31),Level = new LogLevelViewModel(){Level = LogLevel.Debug, Color = Colors.Blue}, Logger = logger4,ThreadId = 1, Message = "Some debug message"});

            
            Loggers = new ObservableCollection<LoggerViewModel>();
            Loggers.Add(loggerViewModelProvider.Root);
        }
        public ObservableCollection<LogItemViewModel> LogItems { get; set; }
        public ObservableCollection<LoggerViewModel> Loggers { get; set; }
        public ICommand ExitCommand { get; set; }

    }
}