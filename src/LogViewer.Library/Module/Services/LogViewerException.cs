using System;

namespace LogViewer.Library.Module.Services
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