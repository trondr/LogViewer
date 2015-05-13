using System.Windows;
using System.Windows.Media;

namespace github.trondr.LogViewer.Library.ViewModels
{
    public class LogLevelViewModel: DependencyObject
    {
        public LogLevelViewModel()
        {
            IsVisible = true;
            Level = LogLevel.Trace;            
        }
        
        public static readonly DependencyProperty LevelProperty = DependencyProperty.Register(
            "Level", typeof (LogLevel), typeof (LogLevelViewModel), new PropertyMetadata(default(LogLevel)));

        public LogLevel Level
        {
            get { return (LogLevel) GetValue(LevelProperty); }
            set { SetValue(LevelProperty, value); }
        }

        public static readonly DependencyProperty ColorProperty = DependencyProperty.Register(
            "Color", typeof (Color), typeof (LogLevelViewModel), new PropertyMetadata(default(Color)));

        public Color Color
        {
            get { return (Color) GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }

        public static readonly DependencyProperty IsVisibleProperty = DependencyProperty.Register(
            "IsVisible", typeof (bool), typeof (LogLevelViewModel), new PropertyMetadata(default(bool)));

        public bool IsVisible
        {
            get { return (bool) GetValue(IsVisibleProperty); }
            set { SetValue(IsVisibleProperty, value); }
        }
    }
}