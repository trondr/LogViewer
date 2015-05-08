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
            LogItems = new ObservableCollection<LogItemViewModel>();
            LogItems.Add(new LogItemViewModel(){Time = DateTime.Now.AddSeconds(-31),Level = new LogLevelViewModel(){Level = LogLevel.Fatal, Color = Colors.Red}, Logger = "Test.SomeClass",ThreadId = 1, Message = "Some fatal error message"});
            LogItems.Add(new LogItemViewModel(){Time = DateTime.Now.AddSeconds(-31),Level = new LogLevelViewModel(){Level = LogLevel.Error, Color = Colors.LightCoral}, Logger = "Test.SomeClass",ThreadId = 1, Message = "Some error message"});
            LogItems.Add(new LogItemViewModel(){Time = DateTime.Now.AddSeconds(-31),Level = new LogLevelViewModel(){Level = LogLevel.Warning, Color = Colors.Orange}, Logger = "Test.SomeClass",ThreadId = 1, Message = "Some warning message"});
            LogItems.Add(new LogItemViewModel(){Time = DateTime.Now.AddSeconds(-31),Level = new LogLevelViewModel(){Level = LogLevel.Info, Color = Colors.DarkSeaGreen}, Logger = "Test.SomeClass",ThreadId = 1, Message = "Some info message"});
            LogItems.Add(new LogItemViewModel(){Time = DateTime.Now.AddSeconds(-31),Level = new LogLevelViewModel(){Level = LogLevel.Trace, Color = Colors.DarkGray}, Logger = "Test.SomeClass",ThreadId = 1, Message = "Some trace message"});
            LogItems.Add(new LogItemViewModel(){Time = DateTime.Now.AddSeconds(-31),Level = new LogLevelViewModel(){Level = LogLevel.Debug, Color = Colors.Blue}, Logger = "Test.SomeClass",ThreadId = 1, Message = "Some debug message"});
        }
        public ObservableCollection<LogItemViewModel> LogItems { get; set; }
        public ICommand ExitCommand { get; set; }

    }
}