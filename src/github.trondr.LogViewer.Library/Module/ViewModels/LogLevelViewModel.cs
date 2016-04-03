using System.Windows;
using System.Windows.Media;
using github.trondr.LogViewer.Library.Module.Model;

namespace github.trondr.LogViewer.Library.Module.ViewModels
{
    public class LogLevelViewModel: DependencyObject
    {
        public LogLevelViewModel()
        {
            IsVisible = true;
            Level = LogLevel.Trace;
            Color = new SolidColorBrush(){Color = Colors.LightSeaGreen};            
        }
        
        public static readonly DependencyProperty LevelProperty = DependencyProperty.Register(
            "Level", typeof (LogLevel), typeof (LogLevelViewModel), new PropertyMetadata(default(LogLevel)));

        public LogLevel Level
        {
            get { return (LogLevel) GetValue(LevelProperty); }
            set { SetValue(LevelProperty, value); }
        }

        public static readonly DependencyProperty IsVisibleProperty = DependencyProperty.Register(
            "IsVisible", typeof (bool), typeof (LogLevelViewModel), new PropertyMetadata(default(bool)));

        public bool IsVisible
        {
            get { return (bool) GetValue(IsVisibleProperty); }
            set { SetValue(IsVisibleProperty, value); }
        }

        public static readonly DependencyProperty ColorProperty = DependencyProperty.Register(
            "Color", typeof (Brush), typeof (LogLevelViewModel), new PropertyMetadata(default(Brush)));

        public Brush Color
        {
            get { return (Brush) GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }
    }
}