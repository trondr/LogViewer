using System;
using System.Linq;
using System.Windows;
using Common.Logging;
using GalaSoft.MvvmLight.Messaging;
using LogViewer.Library.Infrastructure;
using LogViewer.Library.Module.Messages;
using LogViewer.Library.Module.Services;
using LogViewer.Library.Module.Views;

namespace LogViewer.Library.Module.Commands.OpenLog
{
    public class OpenLogCommandProvider : CommandProvider, IOpenLogCommandProvider
    {
        private readonly MainWindow _mainWindow;
        private readonly IMessenger _messenger;
        private readonly ILogItemHandlerFactory _logItemHandlerFactory;
        private readonly ILogItemConnectionProvider _logItemConnectionProvider;
        private readonly ITestWriteLog _testWriteLog;
        private readonly ILog _logger;

        public OpenLogCommandProvider(
            MainWindow mainWindow,
            IMessenger messenger,
            ILogItemHandlerFactory logItemHandlerFactory, 
            ILogItemConnectionProvider logItemConnectionProvider, 
            ITestWriteLog testWriteLog, ILog logger)
        {
            _mainWindow = mainWindow;
            _messenger = messenger;
            _logItemHandlerFactory = logItemHandlerFactory;
            _logItemConnectionProvider = logItemConnectionProvider;
            _testWriteLog = testWriteLog;
            _logger = logger;
        }
        
        public int OpenLogs(string[] connectionStrings)
        {
            //_testWriteLog.StartWritingLog();
            var returnValue = 0;
            //VerifyMainView(_mainWindow);

            //var logItemNotifiable = _mainWindow.ViewModel.SelectedViewModel as ILogItemNotifiable;
            //if (logItemNotifiable != null)
            //{
                //ILogItemHandler[] logItemHandlers = null;
                //var logItemConnections = _logItemConnectionProvider.GetLogItemConnections(connectionStrings).ToList();
                //foreach (var logItemConnection in logItemConnections)
                //{
                //    logItemHandlers = _logItemHandlerFactory.GetLogItemHandlers(logItemConnection);
                //    foreach (var logItemHandler in logItemHandlers)
                //    {
                //        logItemHandler.Connection = logItemConnection;
                //        logItemHandler.ShowFromBeginning = true;
                //        logItemHandler.Initialize();
                //        logItemHandler.Attach(logItemNotifiable);
                //    }
                //}
                var application = new Application();
                _mainWindow.Loaded += (sender, args) =>
                {
                    _messenger.Send(new ConnectionStringsMessage {ConnectionStrings = connectionStrings});
                };
                application.Run(_mainWindow);
                //if (logItemHandlers != null)
                //{
                //    foreach (var logItemHandler in logItemHandlers)
                //    {
                //        logItemHandler.Detach();
                //        logItemHandler.Terminate();
                //    }
                //}
            //}
            //else
            //{
            //    _logger.Fatal("Main view model is null");
            //    returnValue = 1;
            //}
            //_testWriteLog.StopWritingLog();
            return returnValue;
        }

        private void VerifyMainView(MainWindow mainWindow)
        {
            if (mainWindow == null) throw new ArgumentNullException(nameof(mainWindow));
            if (mainWindow.ViewModel == null) throw new ArgumentNullException(nameof(mainWindow.ViewModel));
            if (mainWindow.ViewModel.SelectedViewModel == null) throw new ArgumentNullException(nameof(mainWindow.ViewModel.SelectedViewModel));
        }
    }
}