using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reactive.Linq;
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
using Reactive.Bindings;

namespace LogViewer.Library.Module.ViewModels
{
    public class MainViewModel : ViewModelBase, IMainViewModel, ILogItemNotifiable
    {
        private readonly ILogLevelViewModelProvider _logLevelViewModelProvider;
        private readonly ILoggerViewModelProvider _loggerViewModelProvider;
        private readonly ITypeMapper _typeMapper;
        private readonly ILogViewerConfiguration _configuration;
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
        private ReactiveProperty<string> _reactiveSearchFilter;
        private bool _searchIsCaseSensitive;

        public MainViewModel(
            ILogLevelViewModelProvider logLevelViewModelProvider,
            ILoggerViewModelProvider loggerViewModelProvider,
            ITypeMapper typeMapper,
            IMessenger messenger,
            ILogViewerConfiguration configuration,
            ILogItemConnectionProvider logItemConnectionProvider,
            ILogItemHandlerFactory logItemHandlerFactory,
            ILog logger
            ) : base(messenger)
        {
            _logLevelViewModelProvider = logLevelViewModelProvider;
            _loggerViewModelProvider = loggerViewModelProvider;
            _typeMapper = typeMapper;
            _configuration = configuration;
            _logItemConnectionProvider = logItemConnectionProvider;
            _logItemHandlerFactory = logItemHandlerFactory;
            _logger = logger;            
        }

        public Task LoadAsync()
        {
            SetWindowPosition();
            MessengerInstance.Register<SaveWindowPositionMessage>(this, SaveWindowPosition);
            
            return Task.Run(() =>
            {
                if (LoadStatus == LoadStatus.Loaded || LoadStatus == LoadStatus.Loading || LoadStatus == LoadStatus.UnLoading)
                    return;
                SearchIsCaseSensitive = Properties.Settings.Default.SearchIsCaseSensitive;
                LoadStatus = LoadStatus.Loading;                                
                DispatcherHelper.CheckBeginInvokeOnUI(() => SearchFilter = Properties.Settings.Default.SearchFilter);
                LoadStatus = LoadStatus.Loaded;
                ConfigureAndAttachLogItemHandlers(_configuration.ConnectionStrings);
            });
        }

        private void SetWindowPosition()
        {
            var storedWindowsPlacement = Properties.Settings.Default.WindowPlacement;
            var setWindowPositionMessage = new SetWindowPositionMessage
            {
                WindowPlacement = storedWindowsPlacement
            };
            MessengerInstance.Send(setWindowPositionMessage);
        }

        private void SaveWindowPosition(SaveWindowPositionMessage message)
        {
            var windowPlacement = message.WindowPlacement;
            Properties.Settings.Default.WindowPlacement = windowPlacement;            
            Properties.Settings.Default.Save();
        }
        
        private void ConfigureAndAttachLogItemHandlers(string[] connectionStrings)
        {
            _logger.Info($"Connection string count: '{connectionStrings?.Length}'");
            var logItemNotifiable = this as ILogItemNotifiable;
            var logItemConnections = _logItemConnectionProvider.GetLogItemConnections(connectionStrings).ToList();
            foreach (var logItemConnection in logItemConnections)
            {
                _logger.Info($"Getting log item handler for log item connection '{logItemConnection.Value}'");
                _logItemHandlers = _logItemHandlerFactory.GetLogItemHandlers(logItemConnection);
                foreach (var logItemHandler in _logItemHandlers)
                {
                    logItemHandler.Connection = logItemConnection;
                    logItemHandler.ShowFromBeginning = true;
                    logItemHandler.Initialize();
                    _logger.Info($"Attaching log item handler of type '{logItemHandler.GetType().Name}' to user interface.");
                    DispatcherHelper.CheckBeginInvokeOnUI(() => logItemHandler.Attach(logItemNotifiable));
                }
            }
        }

        public Task UnloadAsync()
        {
            return Task.Run(() =>
            {
                if (LoadStatus == LoadStatus.NotLoaded || LoadStatus == LoadStatus.Loading || LoadStatus == LoadStatus.UnLoading)
                    return;

                LoadStatus = LoadStatus.UnLoading;
                _logger.Info($"Unloading {GetType().Name}");
                Properties.Settings.Default.SearchFilter = SearchFilter;
                Properties.Settings.Default.SearchIsCaseSensitive = SearchIsCaseSensitive;
                Properties.Settings.Default.Save();
                
                LoadStatus = LoadStatus.NotLoaded;
                
                if (_logItemHandlers != null)
                {
                    foreach (var logItemHandler in _logItemHandlers)
                    {
                        logItemHandler.Detach();
                        logItemHandler.Terminate();
                    }
                }
                MessengerInstance.Unregister<SaveWindowPositionMessage>(this);
            });            
        }

        public ICommand ExitCommand
        {
            get { return _exitCommand ?? (_exitCommand = new RelayCommand(() => MessengerInstance?.Send(new CloseWindowMessage()))); }
            set { _exitCommand = value; }
        }

        public LoadStatus LoadStatus { get; set; }

        public ICommand LoadCommand
        {
            get { return _loadCommand ?? (_loadCommand = new RelayCommand(() => LoadAsync().ConfigureAwait(continueOnCapturedContext: false))); }
            set { _loadCommand = value; }
        }

        public ICommand UnLoadCommand
        {
            get { return _unLoadCommand ?? (_unLoadCommand = new RelayCommand(async () => await UnloadAsync().ConfigureAwait(continueOnCapturedContext: false))); }
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
            if (!logItem.Logger.IsVisible || !logItem.LogLevel.IsVisible)
            {
                filterEventArgs.Accepted = false;
            }
            else
            {
                if (string.IsNullOrEmpty(SearchFilter))
                {
                    filterEventArgs.Accepted = true;
                }
                else if (SearchIsCaseSensitive && logItem.Message.Contains(SearchFilter))
                {
                    filterEventArgs.Accepted = true;
                }
                else if (!SearchIsCaseSensitive && CultureInfo.InvariantCulture.CompareInfo.IndexOf(logItem.Message, SearchFilter, CompareOptions.IgnoreCase) >= 0)
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
                return _logItems ?? (LogItems = new ObservableCollection<LogItemViewModel>());
            }
            set { this.SetProperty(ref _logItems, value); }
        }

        public ObservableCollection<LoggerViewModel> Loggers
        {
            get { return _loggers ?? (_loggers = new ObservableCollection<LoggerViewModel> { _loggerViewModelProvider.Root }); }
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
            get { return _clearSearchFilterCommand ?? (_clearSearchFilterCommand = new RelayCommand(() => { SearchFilter = string.Empty; },() => !string.IsNullOrEmpty(SearchFilter))); }
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
            set { this.SetProperty(ref _searchFilter, value, () =>
            {
                ReactiveSearchFilter.Value = _searchFilter;                
            }); }
        }

        public ReactiveProperty<string> ReactiveSearchFilter
        {
            get
            {
                if (_reactiveSearchFilter == null)
                {
                    _reactiveSearchFilter = new ReactiveProperty<string>();
                    ReactiveSearchFilter
                        .Throttle(TimeSpan.FromMilliseconds(500))
                        .ObserveOn(Dispatcher.CurrentDispatcher)
                        .Subscribe(s => LogItemsView.Refresh());
                }
                return _reactiveSearchFilter;
            }
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

        public bool SearchIsCaseSensitive
        {
            get { return _searchIsCaseSensitive; }
            set { this.SetProperty(ref _searchIsCaseSensitive, value, delegate
            {
                if(!string.IsNullOrWhiteSpace(SearchFilter))
                    LogItemsView.Refresh();
            }); }
        }

        public void Notify(LogItem[] logItems)
        {
            foreach (var logItem in logItems)
            {
                var item = logItem;
                if (LoadStatus != LoadStatus.Loaded)
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