using System;
using System.Diagnostics;

namespace github.trondr.LogViewer.Library.Module.Services.EventLogItem
{
    public class EventLogItemHandler : ILogItemHandler<EventLogItemConnection>
    {
        private IEventLogItemConnection _eventLogItemConnection;
        private EventLog _eventLog;
        
        public void Initialize()
        {
            _eventLogItemConnection = GetEventLogConnection();
            _eventLog = new EventLog(_eventLogItemConnection.LogName,_eventLogItemConnection.Machine,_eventLogItemConnection.Source);
            _eventLog.EntryWritten += EventLogEntryWritten;
            _eventLog.EnableRaisingEvents = true;
        }

        public void Terminate()
        {
            if(_eventLog != null)
            {
                _eventLog.Dispose();
                _eventLog = null;
            }
        }

        public void Attach(ILogItemNotifiable logItemNotifiable)
        {
            throw new System.NotImplementedException();
        }

        public void Detach()
        {
            throw new System.NotImplementedException();
        }

        public bool ShowFromBeginning { get; set; }

        public ILogItemConnection Connection { get; set; }

        private void EventLogEntryWritten(object sender, EntryWrittenEventArgs e)
        {
            throw new NotImplementedException();
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