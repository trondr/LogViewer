using System.Windows;
using Common.Logging;
using GalaSoft.MvvmLight.Messaging;
using LogViewer.Library.Infrastructure;
using LogViewer.Library.Module.Messages;
using LogViewer.Library.Module.Views;

namespace LogViewer.Library.Module.Commands.OpenLog
{
    public class OpenLogCommandProvider : CommandProvider, IOpenLogCommandProvider
    {
        private readonly MainWindow _mainWindow;
        private readonly IMessenger _messenger;        
        private readonly ILog _logger;

        public OpenLogCommandProvider(
            MainWindow mainWindow,
            IMessenger messenger,
            ILog logger
            )
        {
            _mainWindow = mainWindow;
            _messenger = messenger;            
            _logger = logger;
        }
        
        public int OpenLogs(string[] connectionStrings)
        {            
            var returnValue = 0;
            
            _logger.Debug("Subscribing to requests for connection strings...");
            _messenger.Register<RequestConnectionStringsMessage>(this, message =>
            {
                _logger.Debug("A request for connection strings has been received. Send the connection strings...");
                _messenger.Send(new ConnectionStringsMessage {ConnectionStrings = connectionStrings});
            });

            var application = new Application();
            application.Run(_mainWindow);
            
            return returnValue;
        }
    }
}