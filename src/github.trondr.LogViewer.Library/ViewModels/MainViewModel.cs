using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using github.trondr.LogViewer.Library.Common.UI;
using github.trondr.LogViewer.Library.Views;

namespace github.trondr.LogViewer.Library.ViewModels
{
    public class MainViewModel : ViewModelBase, IMainViewModel
    {
        public MainViewModel()
        {
            LogItems = new ObservableCollection<LogItemViewModel>();
            ExitCommand = new CommandHandler(this.Exit, true);
        }

        public ObservableCollection<LogItemViewModel> LogItems { get; set; }

        public ICommand ExitCommand { get; set; }

        private void Exit()
        {
            if (MainWindow != null)
            {
                MainWindow.Close();
            }
            else
            {
                throw new Exception("Unable to close main window because reference to the main window has not been set.");
            }
        }        
    }
}