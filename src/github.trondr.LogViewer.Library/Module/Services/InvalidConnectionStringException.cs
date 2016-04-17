using System;

namespace github.trondr.LogViewer.Library.Module.Services
{
    public class InvalidConnectionStringException : Exception
    {
        public InvalidConnectionStringException(string message): base(message)
        {
            
        }

        public InvalidConnectionStringException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}