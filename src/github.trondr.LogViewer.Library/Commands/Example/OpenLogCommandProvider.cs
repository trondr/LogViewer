using System.Windows;
using Common.Logging;
using github.trondr.LogViewer.Library.Infrastructure;
using github.trondr.LogViewer.Library.ViewModels;
using github.trondr.LogViewer.Library.Views;

namespace github.trondr.LogViewer.Library.Commands.Example
{
    public class OpenLogCommandProvider : CommandProvider, IOpenLogCommandProvider
    {
        private readonly MainWindow _mainWindow;        
        private readonly ILog _logger;

        public OpenLogCommandProvider(MainWindow mainWindow, ILog logger)
        {
            _mainWindow = mainWindow;
            _logger = logger;
        }


        public int OpenLog(string logFile)
        {
            var returnValue = 0;
            _logger.Info("Showing main window as an example user interface.");
            var application = new Application();
            application.Run(_mainWindow);
            var viewModel = _mainWindow.View.ViewModel as MainViewModel;
            if (viewModel != null)
            {
                _logger.Info("Getting info from the user interface and do something with it: " + viewModel.LogItems.Count);
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