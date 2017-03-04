using System.Windows.Media;
using LogViewer.Library.Module.Model;

namespace LogViewer.Library.Module.ViewModels
{
    public interface ILogLevelSettings
    {
        SolidColorBrush LoadForegroundColor(LogLevel logLevel);

        void SaveForegroundColor(LogLevel logLevel, SolidColorBrush color);
    }
}