using System;

namespace github.trondr.LogViewer.Library.Module.Services.EventLogItem
{
    public class LogViewerException : Exception
    {
        public LogViewerException(string message) : base(message)
        {            
        }
    }
}