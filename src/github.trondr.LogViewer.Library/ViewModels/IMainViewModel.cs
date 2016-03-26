using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace github.trondr.LogViewer.Library.ViewModels
{
    public interface IMainViewModel
    {
        void Initialize();
        ICollectionView LogItemsView { get; }
        ObservableCollection<LogItemViewModel> LogItems { get; set; }
        ObservableCollection<LoggerViewModel> Loggers { get; set; }
        ObservableCollection<LogLevelViewModel> LogLevels { get; set; }
        ICommand ExitCommand { get; set; }
        ICommand UpdateCommand { get; set; }
        ICommand ClearSearchFilterCommand { get; set; }
        bool IsBusy { get; set; }
        string SearchFilter { get; set; }
        LogItemViewModel SelectedLogItem {get;set; }
        
    }
}
