using System.Collections.Generic;

namespace github.trondr.LogViewer.Library.Services
{
    public interface ILogItemConnectionProvider
    {
        IEnumerable<ILogItemConnection> GetLogItemConnections(string[] connectionStrings);
    }
}