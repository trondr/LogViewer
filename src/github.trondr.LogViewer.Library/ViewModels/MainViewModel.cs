using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using github.trondr.LogViewer.Library.Common.UI;

namespace github.trondr.LogViewer.Library.ViewModels
{
    public class MainViewModel : ViewModelBase, IMainViewModel
    {
        
        public MainViewModel(ILoggerViewModelProvider loggerViewModelProvider)
        {                        
            LogItems = new ObservableCollection<LogItemViewModel>();
            Loggers = new ObservableCollection<LoggerViewModel>();
            Loggers.Add(loggerViewModelProvider.Root);
            ExitCommand = new CommandHandler(this.Exit, true);


            //loggerViewModelProvider.GetLogger("Company.Product.Class1");
            //loggerViewModelProvider.GetLogger("Company.Product.Class2");
            //loggerViewModelProvider.GetLogger("Company.Product.Class3");
            //loggerViewModelProvider.GetLogger("Company.Product.Class4");            
        }

        public ObservableCollection<LogItemViewModel> LogItems { get; set; }

        public ObservableCollection<LoggerViewModel> Loggers { get; set; }

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