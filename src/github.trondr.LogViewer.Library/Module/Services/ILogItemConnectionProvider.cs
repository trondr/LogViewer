using System.Collections.Generic;

namespace github.trondr.LogViewer.Library.Module.Services
{
    public interface ILogItemConnectionProvider
    {
        IEnumerable<ILogItemConnection> GetLogItemConnections(string[] connectionStrings);
    }
}