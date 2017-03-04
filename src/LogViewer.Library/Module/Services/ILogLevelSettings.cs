using System.Windows.Media;
using LogViewer.Library.Module.Model;

namespace LogViewer.Library.Module.Services
{
    public interface ILogLevelSettings
    {
        SolidColorBrush LoadForegroundColor(LogLevel logLevel);

        void SaveForegroundColor(LogLevel logLevel, SolidColorBrush color);
    }
}