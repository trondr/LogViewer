using System.Windows.Media;
using GalaSoft.MvvmLight;
using LogViewer.Library.Module.Common.UI;
using LogViewer.Library.Module.Model;

namespace LogViewer.Library.Module.ViewModels
{
    public class LogLevelViewModel : ViewModelBase, ILogLevelViewModel
    {
        private Brush _color;
        private bool _isVisible;
        private LogLevel _level;

        public LogLevelViewModel()
        {
            IsVisible = true;
            Level = LogLevel.Trace;
            Color = new SolidColorBrush {Color = Colors.LightSeaGreen};
        }

        public Brush Color
        {
            get { return _color; }
            set { this.SetProperty(ref _color, value); }
        }

        public bool IsVisible
        {
            get { return _isVisible; }
            set { this.SetProperty(ref _isVisible, value); }
        }

        public LogLevel Level
        {
            get { return _level; }
            set { this.SetProperty(ref _level, value); }
        }
    }
}