using System.Collections.ObjectModel;
using System.Windows.Input;

namespace github.trondr.LogViewer.Library.ViewModels
{
    public interface IMainViewModel
    {
        ObservableCollection<LogItemViewModel> LogItems { get; set; }

        ObservableCollection<LoggerViewModel> Loggers { get; set; }

        ICommand ExitCommand { get; set; }
    }
}
