using System;

namespace github.trondr.LogViewer.Library.Module.Services
{
    public class UnSupportedConnectionStringException : Exception
    {
        public UnSupportedConnectionStringException(string message): base(message)
        {
            
        }

        public UnSupportedConnectionStringException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}