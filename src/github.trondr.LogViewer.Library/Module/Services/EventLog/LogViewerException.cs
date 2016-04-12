using System;

namespace github.trondr.LogViewer.Library.Module.Services.EventLog
{
    public class LogViewerException : Exception
    {
        public LogViewerException(string message) : base(message)
        {            
        }

        public LogViewerException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}