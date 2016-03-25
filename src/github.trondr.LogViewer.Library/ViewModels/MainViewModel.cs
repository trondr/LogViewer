using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using github.trondr.LogViewer.Library.Common.UI;
using github.trondr.LogViewer.Library.Model;
using github.trondr.LogViewer.Library.Services;

namespace github.trondr.LogViewer.Library.ViewModels
{
    public class MainViewModel : ViewModelBase, IMainViewModel, ILogItemNotifiable
    {
        private readonly IFileLogItemReceiver _fileLogItemReceiver;
        private readonly IRandomLogItemReceiver _randomLogItemReceiver;        
        private readonly IMapper _mapper;
        //private readonly Timer _testDataTimer;
        //bool _cancelTestDataTimer;
        //private readonly AutoResetEvent _autoResetEvent;
        private bool _callBackRegistered;
        //private int _count;
        private readonly CollectionViewSource _logItemsViewSource;


        public MainViewModel(ILoggerViewModelProvider loggerViewModelProvider, ILogLevelViewModelProvider logLevelViewModelProvider, IFileLogItemReceiver fileLogItemReceiver, IRandomLogItemReceiver randomLogItemReceiver, IMapper mapper)
        {
            _fileLogItemReceiver = fileLogItemReceiver;
            _randomLogItemReceiver = randomLogItemReceiver;
            _mapper = mapper;
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
                else if(logItem.Message.Contains(SearchFilter))
                {
                    filterEventArgs.Accepted = true;
                }
                else
                {
                    filterEventArgs.Accepted = false;
                }                
            }
        }

        public void Initialize()
        {
            if (!_callBackRegistered && MainWindow != null)
            {
                MainWindow.Closing += MainWindow_Closing;
                _callBackRegistered = true;
            }
            //_fileLogItemReceiver.LogFileName = @"C:\Users\Public\SKALA\Logs\Ito.Tools.SharedFolder.Client\Ito.Tools.SharedFolder.Client.eta410.log";
            //_fileLogItemReceiver.Terminate();
            //_fileLogItemReceiver.ShowFromBeginning = true;
            //_fileLogItemReceiver.Initialize();
            //_fileLogItemReceiver.Attach(this);   
            
            _randomLogItemReceiver.Terminate();
            _randomLogItemReceiver.Initialize();
            _randomLogItemReceiver.Attach(this);
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
            Properties.Settings.Default.SearchFilter = SearchFilter;
            Properties.Settings.Default.Save();
            this.Terminate();
        }

        public void Notify(LogItem[] logItems)
        {
            foreach (var logItem in logItems)
            {
                var item = logItem;
                Dispatcher.Invoke(() =>
                {
                    LogItems.Add(_mapper.Map<LogItemViewModel>(item));
                });
            }
        }

        public void Notify(LogItem logItem)
        {
             var item = logItem;
            Dispatcher.Invoke(() =>
            {
                LogItems.Add(_mapper.Map<LogItemViewModel>(item));
            });
        }

        public void Terminate()
        {
            _fileLogItemReceiver.Terminate();
            _randomLogItemReceiver.Terminate();
        }
    }
}