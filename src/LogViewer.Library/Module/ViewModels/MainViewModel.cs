using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;
using Common.Logging;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using LogViewer.Library.Infrastructure;
using LogViewer.Library.Module.Common.UI;
using LogViewer.Library.Module.Messages;
using LogViewer.Library.Module.Model;
using LogViewer.Library.Module.Services;

namespace LogViewer.Library.Module.ViewModels
{
    public class MainViewModel : ViewModelBase, IMainViewModel, ILogItemNotifiable
    {
        private readonly ILogLevelViewModelProvider _logLevelViewModelProvider;
        private readonly ILoggerViewModelProvider _loggerViewModelProvider;
        private readonly ITypeMapper _typeMapper;
        private readonly ILogItemConnectionProvider _logItemConnectionProvider;
        private readonly ILogItemHandlerFactory _logItemHandlerFactory;
        private readonly ILog _logger;
        private ICommand _loadCommand;
        private ICommand _unLoadCommand;
        private ICommand _exitCommand;
        private CollectionViewSource _logItemsViewSource;
        private ObservableCollection<LogItemViewModel> _logItems;
        private ObservableCollection<LoggerViewModel> _loggers;
        private ObservableCollection<LogLevelViewModel> _logLevels;
        private ICommand _clearSearchFilterCommand;
        private string _searchFilter;
        private LogItemViewModel _selectedLogItem;
        private bool _logItemIsSelected;
        private bool _isBusy;
        private ICommand _updateCommand;
        private ILogItemHandler[] _logItemHandlers;

        public MainViewModel(
            ILogLevelViewModelProvider logLevelViewModelProvider,
            ILoggerViewModelProvider loggerViewModelProvider,
            ITypeMapper typeMapper,
            IMessenger messenger,
            ILogItemConnectionProvider logItemConnectionProvider,
            ILogItemHandlerFactory logItemHandlerFactory,
            ILog logger
            ) : base(messenger)
        {
            _logLevelViewModelProvider = logLevelViewModelProvider;
            _loggerViewModelProvider = loggerViewModelProvider;
            _typeMapper = typeMapper;
            _logItemConnectionProvider = logItemConnectionProvider;
            _logItemHandlerFactory = logItemHandlerFactory;
            _logger = logger;
        }

        public Task LoadAsync()
        {
            if (LoadStatus == LoadStatus.Loaded || LoadStatus == LoadStatus.Loading || LoadStatus == LoadStatus.UnLoading )
                return Task.FromResult(true);            
            LoadStatus = LoadStatus.Loading;
            _logger.Info($"Loading {this.GetType().Name}");   
            SearchFilter = Properties.Settings.Default.SearchFilter;
            MessengerInstance.Register<ConnectionStringsMessage>(this, message =>
            {
                var logItemNotifiable = this as ILogItemNotifiable;
                var logItemConnections = _logItemConnectionProvider.GetLogItemConnections(message.ConnectionStrings).ToList();
                foreach (var logItemConnection in logItemConnections)
                {
                    _logItemHandlers = _logItemHandlerFactory.GetLogItemHandlers(logItemConnection);
                    foreach (var logItemHandler in _logItemHandlers)
                    {
                        logItemHandler.Connection = logItemConnection;
                        logItemHandler.ShowFromBeginning = true;
                        logItemHandler.Initialize();
                        logItemHandler.Attach(logItemNotifiable);
                    }
                }
            });
            MessengerInstance.Send(new RequestConnectionStringsMessage());
            LoadStatus = LoadStatus.Loaded;
            return Task.FromResult(true);
        }

        public Task UnloadAsync()
        {
            if (LoadStatus == LoadStatus.NotLoaded || LoadStatus == LoadStatus.Loading || LoadStatus == LoadStatus.UnLoading )
                return Task.FromResult(true);
            
            LoadStatus = LoadStatus.UnLoading;
            _logger.Info($"Unloading {this.GetType().Name}"); 
            Properties.Settings.Default.SearchFilter = SearchFilter;
            Properties.Settings.Default.Save();   
            
                                
            LoadStatus = LoadStatus.NotLoaded;
            MessengerInstance.Unregister<ConnectionStringsMessage>(this);

            if (_logItemHandlers != null)
            {
                foreach (var logItemHandler in _logItemHandlers)
                {
                    logItemHandler.Detach();
                    logItemHandler.Terminate();
                }
            }
            return Task.FromResult(true);
        }

        public ICommand ExitCommand
        {
            get { return _exitCommand ?? (_exitCommand = new RelayCommand(() => MessengerInstance?.Send(new CloseWindowMessage()))); }
            set { _exitCommand = value; }
        }

        public LoadStatus LoadStatus { get; set; }

        public ICommand LoadCommand
        {
            get { return _loadCommand ?? (_loadCommand = new RelayCommand(async () => await LoadAsync())); }
            set { _loadCommand = value; }
        }

        public ICommand UnLoadCommand
        {
            get { return _unLoadCommand ?? (_unLoadCommand = new RelayCommand(async () => await UnloadAsync())); }
            set { _unLoadCommand = value; }
        }
        
        public ICollectionView LogItemsView
        {
            get
            {
                if (_logItemsViewSource == null)
                {
                    _logItemsViewSource = new CollectionViewSource { Source = LogItems };
                    _logItemsViewSource.Filter += LogitemsViewSourceOnFilter;
                }
                return _logItemsViewSource.View;
            }
        }

        private void LogitemsViewSourceOnFilter(object sender, FilterEventArgs filterEventArgs)
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

        public ObservableCollection<LogItemViewModel> LogItems
        {
            get
            {
                if (_logItems == null)
                {
                    LogItems = new ObservableCollection<LogItemViewModel>();
                    LogItems.CollectionChanged += LogItemsOnCollectionChanged;
                }
                return _logItems;
            }
            set { this.SetProperty(ref _logItems, value); }
        }

        private void LogItemsOnCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            ScrollToBottom?.Invoke();
        }

        public ObservableCollection<LoggerViewModel> Loggers
        {
            get
            {
                if (_loggers == null)
                {
                    Loggers = new ObservableCollection<LoggerViewModel>() { _loggerViewModelProvider.Root };
                }
                return _loggers ?? (_loggers = new ObservableCollection<LoggerViewModel>());
            }
            set { this.SetProperty(ref _loggers, value); }
        }

        public ObservableCollection<LogLevelViewModel> LogLevels
        {
            get
            {
                if (_logLevels == null)
                {
                    var traceLogLevel = _logLevelViewModelProvider.GetLevel("Trace");
                    var debugLogLevel = _logLevelViewModelProvider.GetLevel("Debug");
                    var infoLogLevel = _logLevelViewModelProvider.GetLevel("Info");
                    var warnLogLevel = _logLevelViewModelProvider.GetLevel("Warn");
                    var errorLogLevel = _logLevelViewModelProvider.GetLevel("Error");
                    var fatalLogLevel = _logLevelViewModelProvider.GetLevel("Fatal");
                    LogLevels = new ObservableCollection<LogLevelViewModel> { traceLogLevel, debugLogLevel, infoLogLevel, warnLogLevel, errorLogLevel, fatalLogLevel };
                }
                return _logLevels;
            }            
            set { this.SetProperty(ref _logLevels, value); }
        }

        public ICommand UpdateCommand
        {
            get { return _updateCommand ?? (_updateCommand = new RelayCommand(Update)); }
            set { _updateCommand = value; }
        }

        private void Update()
        {
            IsBusy = true;
            _logItemsViewSource.View.Refresh();
            IsBusy = false;
        }

        public ICommand ClearSearchFilterCommand
        {
            get { return _clearSearchFilterCommand ?? (_clearSearchFilterCommand = new RelayCommand(() => { SearchFilter = string.Empty;})); }
            set { _clearSearchFilterCommand = value; }
        }

        public bool IsBusy
        {
            get { return _isBusy; }
            set { this.SetProperty(ref _isBusy, value); }
        }

        public string SearchFilter
        {
            get { return _searchFilter ?? (SearchFilter = Properties.Settings.Default.SearchFilter); }
            set { this.SetProperty(ref _searchFilter, value, () => {LogItemsView.Refresh();}); }
        }

        public LogItemViewModel SelectedLogItem
        {
            get { return _selectedLogItem; }
            set { this.SetProperty(ref _selectedLogItem, value, () => { LogItemIsSelected = (_selectedLogItem != null); }); }
        }

        public bool LogItemIsSelected
        {
            get { return _logItemIsSelected; }
            set { this.SetProperty(ref _logItemIsSelected, value); }
        }

        public event Action ScrollToBottom;
        
        public void Notify(LogItem[] logItems)
        {
            foreach (var logItem in logItems)
            {
                var item = logItem;
                if(LoadStatus != LoadStatus.Loaded)
                {
                    break;
                }

                try
                {
                    DispatcherHelper.CheckBeginInvokeOnUI(() =>
                    {
                        var logItemViewModel = _typeMapper.Map<LogItemViewModel>(item);
                        LogItems.Add(logItemViewModel);
                    });
                }
                catch (TaskCanceledException)
                {
                    //Swallow
                }                
            }
        }

        public void Notify(LogItem logItem)
        {
            var item = logItem;
            DispatcherHelper.CheckBeginInvokeOnUI(() =>
            {
                var logItemViewModel = _typeMapper.Map<LogItemViewModel>(item);
                LogItems.Add(logItemViewModel);
            });
        }
    }
}