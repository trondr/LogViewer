using System.Collections.ObjectModel;

namespace LogViewer.Library.Module.ViewModels
{
    public interface ILoggerViewModel
    {
        ObservableCollection<LoggerViewModel> Children { get; set; }
        string DisplayName { get; set; }
        bool IsVisible { get; set; }
        string Name { get; set; }
        LoggerViewModel Parent { get; set; }
    }
}