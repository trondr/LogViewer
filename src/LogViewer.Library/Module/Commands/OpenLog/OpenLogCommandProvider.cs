using System.Windows;
using Common.Logging;
using LogViewer.Library.Infrastructure;
using LogViewer.Library.Module.Services;
using LogViewer.Library.Module.Views;

namespace LogViewer.Library.Module.Commands.OpenLog
{
    public class OpenLogCommandProvider : CommandProvider, IOpenLogCommandProvider
    {
        private readonly MainWindow _mainWindow;
        private readonly ILogViewerConfiguration _configuration;             
        private readonly ILog _logger;

        public OpenLogCommandProvider(
            MainWindow mainWindow,
            ILogViewerConfiguration configuration,
            ILog logger
            )
        {
            _mainWindow = mainWindow;
            _configuration = configuration;                      
            _logger = logger;
        }
        
        public int OpenLogs(string[] connectionStrings)
        {            
            var returnValue = 0;
            _logger.Info($"Getting connection strings: '{string.Join(";", connectionStrings)}'");
            _configuration.ConnectionStrings = connectionStrings;
            var application = new Application();            
            _logger.Info("Starting user interface...");
            _mainWindow.WindowState = WindowState.Minimized;
            application.Run(_mainWindow);
            return returnValue;
        }
    }
}