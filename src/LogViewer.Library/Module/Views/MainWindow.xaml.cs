using System;
using System.Windows;
using Common.Logging;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using LogViewer.Library.Infrastructure;
using LogViewer.Library.Module.Messages;
using LogViewer.Library.Module.ViewModels;

namespace LogViewer.Library.Module.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainWindowViewModel _viewModel;

        public ILog Logger { get; set; }

        public IMessenger Messenger { get; set; }

        public MainWindowViewModel ViewModel
        {
            get { return _viewModel; }
            set
            {
                _viewModel = value;
                if(_viewModel != null)
                    this.DataContext = _viewModel;
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            this.Title = ApplicationInfoHelper.ApplicationName + " " + ApplicationInfoHelper.ApplicationVersion;
            Loaded += OnLoaded;
            Unloaded += OnUnloaded;
        }

        void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (ViewModelBase.IsInDesignModeStatic) return;
            Logger?.Debug("MainWindow is loaded.");
            if(ViewModel == null)
                throw new NullReferenceException($"ViewModel has not been initialized. Has {typeof(MainWindowViewModel).Namespace} been registered with the container?");
            if(Messenger == null)
                throw new NullReferenceException($"Messenger has not been initialized. Has {typeof(IMessenger).Namespace} been registered with the container?");
            Messenger?.Register<CloseWindowMessage>(this, message => Close());
            DispatcherHelper.Initialize();
        }

        private void OnUnloaded(object sender, RoutedEventArgs routedEventArgs)
        {
            Messenger?.Unregister<CloseWindowMessage>(this);
        }
    }
}
