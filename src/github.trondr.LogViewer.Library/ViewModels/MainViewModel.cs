using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using github.trondr.LogViewer.Library.Common.UI;
using github.trondr.LogViewer.Library.Services;

namespace github.trondr.LogViewer.Library.ViewModels
{
   
    public class MainViewModel : ViewModelBase, IMainViewModel
    {
        private readonly ILogLevelViewModelProvider _logLevelViewModelProvider;
        private readonly ILogItemService _logItemService;
        private Timer _testDataTimer;
        bool _cancelTestDataTimer;
        private AutoResetEvent _autoResetEvent;

        public MainViewModel(ILoggerViewModelProvider loggerViewModelProvider,ILogLevelViewModelProvider logLevelViewModelProvider, ILogItemService logItemService)
        {
            _logLevelViewModelProvider = logLevelViewModelProvider;
            _logItemService = logItemService;
            LogItems = new ObservableCollection<LogItemViewModel>();
            Loggers = new ObservableCollection<LoggerViewModel>();
            LogLevels = new ObservableCollection<LogLevelViewModel>();
            Loggers.Add(loggerViewModelProvider.Root);
                        
            var traceLogLevel = logLevelViewModelProvider.GetLevel("Trace");
            var debugLogLevel = logLevelViewModelProvider.GetLevel("Debug");debugLogLevel.Color = Colors.LightSeaGreen;
            var infoLogLevel = logLevelViewModelProvider.GetLevel("Info");
            var warnLogLevel = logLevelViewModelProvider.GetLevel("Warn");
            var errorLogLevel = logLevelViewModelProvider.GetLevel("Error");
            var fatalLogLevel = logLevelViewModelProvider.GetLevel("Fatal");
            LogLevels = new ObservableCollection<LogLevelViewModel>() { traceLogLevel, debugLogLevel, infoLogLevel, warnLogLevel, errorLogLevel, fatalLogLevel };
            
            ExitCommand = new CommandHandler(this.Exit, true);
            _autoResetEvent = new AutoResetEvent(false);
            _testDataTimer = new Timer(TestDataCallback, _autoResetEvent, 1000, Timeout.Infinite);

        }

        private void TestDataCallback(object state)
        {        
            Console.Write(".");
            MainWindow.Closing -= MainWindow_Closing;
            MainWindow.Closing += MainWindow_Closing;
            var autoResetEvent = (AutoResetEvent) state;            
            Dispatcher.Invoke(() =>
            {
                foreach (var logItemViewModel in _logItemService.GetLogs())
                {
                    LogItems.Add(logItemViewModel);
                }    
            });
            if(_cancelTestDataTimer)
            {
                autoResetEvent.Set();
                return;
            }
            _testDataTimer.Change(1000, Timeout.Infinite);
        }

        public ObservableCollection<LogItemViewModel> LogItems { get; set; }
        public ObservableCollection<LoggerViewModel> Loggers { get; set; }
        public ObservableCollection<LogLevelViewModel> LogLevels { get; set; }

        public ICommand ExitCommand { get; set; }

        private void Exit()
        {
            if (MainWindow != null)
            {                                 
                MainWindow.Close();                
            }
            else
            {
                throw new Exception("Unable to close main window because reference to the main window has not been set.");
            }
        }

        void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if(!_cancelTestDataTimer)
            {
                _cancelTestDataTimer = true;
                _autoResetEvent.WaitOne(5000);
                _testDataTimer.Dispose();
            }
        }        
    }
}