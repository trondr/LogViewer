using System.Windows.Media;
using LogViewer.Library.Module.Model;

namespace LogViewer.Library.Module.ViewModels
{
    public interface ILogLevelViewModel
    {
        Brush Color { get; set; }
        bool IsVisible { get; set; }
        LogLevel Level { get; set; }
    }
}