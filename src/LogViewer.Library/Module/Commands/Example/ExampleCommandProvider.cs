using System.Windows;
using Common.Logging;
using LogViewer.Library.Infrastructure;
using LogViewer.Library.Module.ViewModels;
using LogViewer.Library.Module.Views;

namespace LogViewer.Library.Module.Commands.Example
{
    public class ExampleCommandProvider : CommandProvider, IExampleCommandProvider
    {
        private readonly MainWindow _mainWindow;        
        private readonly ILog _logger;

        public ExampleCommandProvider(MainWindow mainWindow, ILog logger)
        {
            _mainWindow = mainWindow;
            _logger = logger;
        }


        public int Create(string targetRootFolder)
        {
            var returnValue = 1;
            _logger.Info("Showing main window as an example user interface.");
            var application = new Application();
            application.Run(_mainWindow);
            var viewModel = _mainWindow.ViewModel as MainWindowViewModel;
            if (viewModel != null)
            {
                _logger.Info("Getting info from the user interface and do something with it: " + viewModel.SelectedViewModel);
            }
            else
            {
                _logger.Fatal("Fatal error. MainView model returned from dialog was null");
                returnValue = 2;
            }            
            return returnValue;
        }
    }
}