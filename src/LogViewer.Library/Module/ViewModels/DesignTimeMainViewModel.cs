using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using LogViewer.Library.Module.Common.UI;
using LogViewer.Library.Module.Services;

namespace LogViewer.Library.Module.ViewModels
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

            var logLevelViewModelProvider = new LogLevelViewModelProvider(new LogLevelSettings());
            
            var traceLogLevel = logLevelViewModelProvider.GetLevel("Trace");
            var debugLogLevel = logLevelViewModelProvider.GetLevel("Debug");
            var infoLogLevel = logLevelViewModelProvider.GetLevel("Info");
            var warnLogLevel = logLevelViewModelProvider.GetLevel("Warn");
            var errorLogLevel = logLevelViewModelProvider.GetLevel("Error");
            var fatalLogLevel = logLevelViewModelProvider.GetLevel("Fatal");

            LogItems = new ObservableCollection<LogItemViewModel>
            {
                new LogItemViewModel
                {
                    Time = DateTime.Now.AddSeconds(-31),
                    LogLevel = fatalLogLevel,
                    Logger = logger1,
                    ThreadId = "1",
                    Message = "Some fatal error message" + Environment.NewLine + "Next Line"
                },
                new LogItemViewModel
                {
                    Time = DateTime.Now.AddSeconds(-31),
                    LogLevel = errorLogLevel,
                    Logger = logger2,
                    ThreadId = "1",
                    Message = "Some error message" + Environment.NewLine + "Next Line"
                },
                new LogItemViewModel
                {
                    Time = DateTime.Now.AddSeconds(-31),
                    LogLevel = warnLogLevel,
                    Logger = logger3,
                    ThreadId = "1",
                    Message = "Some warning message" + Environment.NewLine + "Next Line"
                },
                new LogItemViewModel
                {
                    Time = DateTime.Now.AddSeconds(-31),
                    LogLevel = infoLogLevel,
                    Logger = logger4,
                    ThreadId = "1",
                    Message = "Some info message" + Environment.NewLine + "Next Line"
                },
                new LogItemViewModel
                {
                    Time = DateTime.Now.AddSeconds(-31),
                    LogLevel = traceLogLevel,
                    Logger = logger4,
                    ThreadId = "1",
                    Message = "Some trace message" + Environment.NewLine + "Next Line"
                },
                new LogItemViewModel
                {
                    Time = DateTime.Now.AddSeconds(-31),
                    LogLevel = debugLogLevel,
                    Logger = logger4,
                    ThreadId = "1",
                    Message = "Some debug message" + Environment.NewLine + "Next Line"
                }
            };
            Loggers = new ObservableCollection<LoggerViewModel> {loggerViewModelProvider.Root};
            LogLevels = new ObservableCollection<LogLevelViewModel> { traceLogLevel, debugLogLevel, infoLogLevel, warnLogLevel, errorLogLevel, fatalLogLevel };
            var collectionViewSource = new CollectionViewSource { Source = LogItems };
            LogItemsView = collectionViewSource.View;
            SelectedLogItem = new LogItemViewModel
            {
                Time = DateTime.Now,
                LogLevel = errorLogLevel,
                Logger = logger3,
                Message = "Some message with \n line shifts",                
            };      
            SearchFilter = string.Empty;
            LogItemIsSelected = true;
        }

        
        
        public async Task LoadAsync()
        {
            await Task.FromResult(true);
        }

        public async Task UnloadAsync()
        {
            await Task.FromResult(true);
        }

        public LoadStatus LoadStatus { get; set; }
        public ICommand ExitCommand { get; set; }
        public ICommand LoadCommand { get; set; }
        public ICommand UnLoadCommand { get; set; }
        public ICollectionView LogItemsView { get; }
        public ObservableCollection<LogItemViewModel> LogItems { get; set; }
        public ObservableCollection<LoggerViewModel> Loggers { get; set; }
        public ObservableCollection<LogLevelViewModel> LogLevels { get; set; }
        public ICommand UpdateCommand { get; set; }
        public ICommand ClearSearchFilterCommand { get; set; }
        public bool IsBusy { get; set; }
        public string SearchFilter { get; set; }
        public LogItemViewModel SelectedLogItem { get; set; }
        public bool LogItemIsSelected { get; set; }

        public bool SearchIsCaseSensitive { get; set; }
    }
}