using System.Windows.Media;
using github.trondr.LogViewer.Library.Model;

namespace github.trondr.LogViewer.Library.ViewModels
{
    public interface ILogLevelSettings
    {
        SolidColorBrush LoadForegroundColor(LogLevel logLevel);

        void SaveForegroundColor(LogLevel logLevel, SolidColorBrush color);
    }
}