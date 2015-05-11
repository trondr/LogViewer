using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using github.trondr.LogViewer.Library.Common.UI;
using github.trondr.LogViewer.Library.Services;

namespace github.trondr.LogViewer.Library.ViewModels
{
   
    public class MainViewModel : ViewModelBase, IMainViewModel
    {
        private readonly ILogItemService _logItemService;
        private Timer _testDataTimer;
        bool _cancelTestDataTimer;
        private AutoResetEvent _autoResetEvent;

        public MainViewModel(ILoggerViewModelProvider loggerViewModelProvider, ILogItemService logItemService)
        {
            _logItemService = logItemService;
            LogItems = new ObservableCollection<LogItemViewModel>();
            Loggers = new ObservableCollection<LoggerViewModel>();
            Loggers.Add(loggerViewModelProvider.Root);
            ExitCommand = new CommandHandler(this.Exit, true);
            _autoResetEvent = new AutoResetEvent(false);
            _testDataTimer = new Timer(TestDataCallback, _autoResetEvent, 1000, Timeout.Infinite);   
        }

        private void TestDataCallback(object state)
        {        
            Console.WriteLine(".");
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