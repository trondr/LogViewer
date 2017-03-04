using System.Windows.Input;
using GalaSoft.MvvmLight;
using LogViewer.Library.Module.Common.UI;

namespace LogViewer.Library.Module.ViewModels
{
    public interface IMainWindowsViewModel: ILoadable
    {
        ViewModelBase SelectedViewModel { get; set; }

        ICommand LoadCommand { get; set; }

        ICommand UnLoadCommand { get; set; }
    }
}