using System;

namespace LogViewer.Library.Module.Services
{
    public class DuplicateConnectionStringException : Exception
    {
        public DuplicateConnectionStringException(string message): base(message)
        {
            
        }

        public DuplicateConnectionStringException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}