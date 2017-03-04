using System.Collections.Generic;

namespace LogViewer.Library.Module.Services
{
    public interface ILogItemConnectionProvider
    {
        IEnumerable<ILogItemConnection> GetLogItemConnections(string[] connectionStrings);
    }
}