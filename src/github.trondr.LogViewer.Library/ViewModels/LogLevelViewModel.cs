using System.Windows;
using System.Windows.Media;

namespace github.trondr.LogViewer.Library.ViewModels
{
    public class LogLevelViewModel: DependencyObject
    {
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
    }
}