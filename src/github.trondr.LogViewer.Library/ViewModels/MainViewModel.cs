using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using github.trondr.LogViewer.Library.Common.UI;
using github.trondr.LogViewer.Library.Services;

namespace github.trondr.LogViewer.Library.ViewModels
{
    public class MainViewModel : ViewModelBase, IMainViewModel
    {
        private readonly ILogItemService _logItemService;        
        private readonly Timer _testDataTimer;
        bool _cancelTestDataTimer;
        private readonly AutoResetEvent _autoResetEvent;
        private bool _callBackRegistered;
        private int _count;
        private readonly CollectionViewSource _logItemsViewSource;


        public MainViewModel(ILoggerViewModelProvider loggerViewModelProvider,ILogLevelViewModelProvider logLevelViewModelProvider, ILogItemService logItemService)
        {
            _logItemService = logItemService;
            LogItems = new ObservableCollection<LogItemViewModel>();
            _logItemsViewSource = new CollectionViewSource { Source = LogItems };
            _logItemsViewSource.Filter+=LogItemsViewSourceOnFilter;
            Loggers = new ObservableCollection<LoggerViewModel>();
            LogLevels = new ObservableCollection<LogLevelViewModel>();
            Loggers.Add(loggerViewModelProvider.Root);
                        
            var traceLogLevel = logLevelViewModelProvider.GetLevel("Trace");
            var debugLogLevel = logLevelViewModelProvider.GetLevel("Debug");
            var infoLogLevel = logLevelViewModelProvider.GetLevel("Info");
            var warnLogLevel = logLevelViewModelProvider.GetLevel("Warn");
            var errorLogLevel = logLevelViewModelProvider.GetLevel("Error");
            var fatalLogLevel = logLevelViewModelProvider.GetLevel("Fatal");
            
            LogLevels = new ObservableCollection<LogLevelViewModel>() { traceLogLevel, debugLogLevel, infoLogLevel, warnLogLevel, errorLogLevel, fatalLogLevel };
            ExitCommand = new CommandHandler(this.Exit, true);
            UpdateCommand = new AsyncCommand(Update, () => !IsBusy);
            ClearSearchFilterCommand = new CommandHandler(delegate { SearchFilter = string.Empty; }, true);
            _autoResetEvent = new AutoResetEvent(false);
            _testDataTimer = new Timer(TestDataCallback, _autoResetEvent, 1000, Timeout.Infinite);
            SearchFilter = Properties.Settings.Default.SearchFilter;
        }

        private Task Update()
        {
            return Task.Factory.StartNew(() => DispatcherService.Invoke(() =>
            {
                IsBusy = true;
                _logItemsViewSource.View.Refresh();
                IsBusy = false;
            }));
        }

        private void LogItemsViewSourceOnFilter(object sender, FilterEventArgs filterEventArgs)
        {
            var logItem = (LogItemViewModel)filterEventArgs.Item;
            if(!logItem.Logger.IsVisible || !logItem.LogLevel.IsVisible)
            {
                filterEventArgs.Accepted = false;
            }
            else
            {
                if(string.IsNullOrEmpty(SearchFilter))
                {
                    filterEventArgs.Accepted = true;
                }
                else if(logItem.Message.Contains(SearchFilter.ToLower()))
                {
                    filterEventArgs.Accepted = true;
                }
                else
                {
                    filterEventArgs.Accepted = false;    
                }                
            }
        }

        private void TestDataCallback(object state)
        {        
            Console.Write(".");
            if(!_callBackRegistered && MainWindow != null)
            { 
                MainWindow.Closing += MainWindow_Closing;
                _callBackRegistered = true;
            }
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
            if(_count < 10)
            { 
                _testDataTimer.Change(100, Timeout.Infinite);
                _count++;
            }
        }

        public ICollectionView LogItemsView { get{return _logItemsViewSource.View;} }
        public ObservableCollection<LogItemViewModel> LogItems { get; set; }
        public ObservableCollection<LoggerViewModel> Loggers { get; set; }
        public ObservableCollection<LogLevelViewModel> LogLevels { get; set; }
        public ICommand ExitCommand { get; set; }
        public ICommand UpdateCommand { get; set; }
        public ICommand ClearSearchFilterCommand { get; set; }

        public static readonly DependencyProperty IsBusyProperty = DependencyProperty.Register(
            "IsBusy", typeof (bool), typeof (MainViewModel), new PropertyMetadata(default(bool)));

        public bool IsBusy
        {
            get { return (bool) GetValue(IsBusyProperty); }
            set { SetValue(IsBusyProperty, value); }
        }

        public static readonly DependencyProperty SearchFilterProperty = DependencyProperty.Register(
            "SearchFilter", typeof (string), typeof (MainViewModel), new FrameworkPropertyMetadata(default(string),SearchFilterChanged));

        private static void SearchFilterChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var mainViewModel = (MainViewModel)dependencyObject;
            mainViewModel.LogItemsView.Refresh();
        }

        public string SearchFilter
        {
            get { return (string) GetValue(SearchFilterProperty); }
            set { SetValue(SearchFilterProperty, value); }
        }

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
                Properties.Settings.Default.SearchFilter = SearchFilter;
                Properties.Settings.Default.Save();
                _cancelTestDataTimer = true;
                _autoResetEvent.WaitOne(5000);
                _testDataTimer.Dispose();
            }
        }        
    }
}