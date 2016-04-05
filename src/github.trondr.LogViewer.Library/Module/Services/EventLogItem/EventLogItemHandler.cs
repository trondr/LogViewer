using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security;
using github.trondr.LogViewer.Library.Module.Model;

namespace github.trondr.LogViewer.Library.Module.Services.EventLogItem
{
    public class EventLogItemHandler : ILogItemHandler<EventLogItemConnection>
    {
        private readonly ILogItemFactory _logItemFactory;
        private IEventLogItemConnection _eventLogItemConnection;
        private EventLog _eventLog;
        private string _baseLoggerName;
        private ILogItemNotifiable _logItemNotifiable;
        private object _sync = new object();

        public EventLogItemHandler(ILogItemFactory logItemFactory)
        {
            _logItemFactory = logItemFactory;
        }

        public void Initialize()
        {
            try
            {
                _eventLogItemConnection = GetEventLogConnection();
                _eventLog = new EventLog(_eventLogItemConnection.LogName, _eventLogItemConnection.Machine, _eventLogItemConnection.Source);
                _eventLog.EntryWritten += EventLogEntryWritten;
                _eventLog.EnableRaisingEvents = true;
                _baseLoggerName = GetBaseLoggerName(_eventLogItemConnection);
            }
            catch (SecurityException ex)
            {
                throw new LogViewerException("Viewing the Windows Eventlog requires administrative priviliges. Please run LogViewer as Administrator.", ex);
            }            
        }

        public void Terminate()
        {
            if (_eventLog != null)
            {
                _eventLog.Dispose();
                _eventLog = null;
            }
        }

        public void Attach(ILogItemNotifiable logItemNotifiable)
        {
            lock(_sync)
            {
                _logItemNotifiable = logItemNotifiable;
            }
        }

        public void Detach()
        {
            lock(_sync)
            {
                _logItemNotifiable = null;
            }
        }

        public bool ShowFromBeginning { get; set; }

        public ILogItemConnection Connection { get; set; }

        private void EventLogEntryWritten(object sender, EntryWrittenEventArgs e)
        {
            if (_logItemNotifiable != null)
            {
                var logItem = GetLogItem(e);
                _logItemNotifiable.Notify(logItem);
                ReleaseLogItem(logItem);
            }
        }

        private string GetBaseLoggerName(IEventLogItemConnection eventLogItemConnection)
        {
            if (!string.IsNullOrEmpty(eventLogItemConnection.Machine) && eventLogItemConnection.Machine != "localhost" && eventLogItemConnection.Machine != ".")
            {
                return string.Format("{0}.{1}", eventLogItemConnection.Machine, eventLogItemConnection.LogName);
            }
            return eventLogItemConnection.LogName;
        }

        private LogItem GetLogItem(EntryWrittenEventArgs e)
        {
            var loggerName = string.IsNullOrEmpty(e.Entry.Source) ? _baseLoggerName : string.Format("{0}.{1}", _baseLoggerName, e.Entry.Source);
            var message = e.Entry.Message;
            var timeStamp = e.Entry.TimeGenerated;
            var logLevel = GetLogLevel(e.Entry.EntryType);
            var category = e.Entry.Category;
            var userName = e.Entry.UserName;
            var threadId = e.Entry.InstanceId.ToString();
            var logItem = _logItemFactory.GetLogItem(timeStamp, logLevel, loggerName, threadId, message, string.Empty);
            if (!string.IsNullOrEmpty(category))
                logItem.Properties.Add("Category", category);
            if (!string.IsNullOrEmpty(userName))
                logItem.Properties.Add("User Name", category);
            return logItem;
        }
        
        void ReleaseLogItem(LogItem logItem)
        {
            _logItemFactory.Release(logItem);
        }

        private LogLevel GetLogLevel(EventLogEntryType entryType)
        {
            switch (entryType)
            {
                case EventLogEntryType.Error:
                    return LogLevel.Error;
                case EventLogEntryType.Warning:
                    return LogLevel.Warn;
                case EventLogEntryType.Information:
                    return LogLevel.Info;
                case EventLogEntryType.SuccessAudit:
                    return LogLevel.Info;
                case EventLogEntryType.FailureAudit:
                    return LogLevel.Error;
                default:
                    return LogLevel.None;
            }
        }

        private IEventLogItemConnection GetEventLogConnection()
        {
            var eventLogConnection = Connection as IEventLogItemConnection;
            ValidateEventLogConnection(eventLogConnection);
            return eventLogConnection;
        }

        private void ValidateEventLogConnection(IEventLogItemConnection eventLogConnection)
        {
            if (eventLogConnection == null) throw new ArgumentNullException(nameof(eventLogConnection));
            if (string.IsNullOrEmpty(eventLogConnection.LogName)) throw new LogViewerException("Log name has not been initialized.");
            if (string.IsNullOrEmpty(eventLogConnection.Machine)) throw new LogViewerException("Machine has not been initialized.");
            if (string.IsNullOrEmpty(eventLogConnection.Source)) throw new LogViewerException("Source has not been initialized.");
        }
    }
}