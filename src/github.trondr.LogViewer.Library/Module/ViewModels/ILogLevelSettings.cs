using System.Windows.Media;
using github.trondr.LogViewer.Library.Module.Model;

namespace github.trondr.LogViewer.Library.Module.ViewModels
{
    public interface ILogLevelSettings
    {
        SolidColorBrush LoadForegroundColor(LogLevel logLevel);

        void SaveForegroundColor(LogLevel logLevel, SolidColorBrush color);
    }
}