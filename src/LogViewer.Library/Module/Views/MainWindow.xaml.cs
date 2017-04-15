using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Forms;
using Common.Logging;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using LogViewer.Library.Infrastructure;
using LogViewer.Library.Module.Common.UI;
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
            Closing += OnClosing;
            Closed+=OnClosed;
        }

        private void OnClosed(object sender, EventArgs eventArgs)
        {
            Messenger?.Unregister<SetWindowPositionMessage>(this);
            Messenger?.Unregister<CloseWindowMessage>(this);
        }

        private void OnClosing(object sender, CancelEventArgs e)
        {
            var saveWindowPossitionMessage = GetSaveWindowPossitionMessage();
            Messenger?.Send(saveWindowPossitionMessage);            
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
            Messenger?.Register<SetWindowPositionMessage>(this, SetWindowPostion);
            DispatcherHelper.Initialize();
        }
        
        private void SetWindowPostion(SetWindowPositionMessage message)
        {
            var verifiedPostion = VerifyPosition(message.Position);

            Top = verifiedPostion.Top;
            Left = verifiedPostion.Left;
            Height = verifiedPostion.Height > 0 ? verifiedPostion.Height : Height;
            Width = verifiedPostion.Width > 0 ? verifiedPostion.Width : Width;
            if (verifiedPostion.Maximized)
            {
                WindowState = WindowState.Maximized;
            }
        }

        private WindowPosition VerifyPosition(WindowPosition position)
        {
            return position;
            var screens = Screen.AllScreens;
            var tempPostion = position;
            foreach (var screen in screens)
            {                
                var formTopLeft = new System.Drawing.Point((int)tempPostion.Left, (int)tempPostion.Top);
                if (screen.WorkingArea.Contains(formTopLeft))
                {
                    return position;
                }
                tempPostion.Left -= screen.Bounds.Width;
                tempPostion.Top -= screen.Bounds.Height;
            }
            return WindowPosition.Default;


            //var converter = new ScreenBoundsConverter(this);
            //foreach (var screen in System.Windows.Forms.Screen.AllScreens)
            //{
            //    var bounds = converter.ConvertBounds(screen.Bounds);
            //    var isWithinBounds = WindowPostionIsWithinBounds(position, bounds);
            //    if (isWithinBounds)
            //        return position;
            //}
            //return WindowPosition.Default;
        }

        private bool WindowPostionIsWithinBounds(WindowPosition position, Rect bounds)
        {
            if (position.Top < bounds.Top || position.Top > bounds.Height)
            {
                return false;
            }
            if (position.Left < bounds.Left || position.Left > bounds.Width)
            {
                return false;
            }
            return true;
        }
        
        private SaveWindowPositionMessage GetSaveWindowPossitionMessage()
        {
            var saveWindowPossitionMessage = new SaveWindowPositionMessage();
            if (WindowState == WindowState.Maximized)
            {
                saveWindowPossitionMessage.Position.Top = RestoreBounds.Top;
                saveWindowPossitionMessage.Position.Left = RestoreBounds.Left;
                saveWindowPossitionMessage.Position.Height = RestoreBounds.Height;
                saveWindowPossitionMessage.Position.Width = RestoreBounds.Width;
                saveWindowPossitionMessage.Position.Maximized = true;
            }
            else
            {
                saveWindowPossitionMessage.Position.Top = Top;
                saveWindowPossitionMessage.Position.Left = Left;
                saveWindowPossitionMessage.Position.Height = Height;
                saveWindowPossitionMessage.Position.Width = Width;
                saveWindowPossitionMessage.Position.Maximized = false;
            }
            return saveWindowPossitionMessage;
        }
    }
}
