using System;
using Common.Logging;

namespace github.trondr.LogViewer.Infrastructure
{
    public interface ILogFactory
    {
        ILog GetLogger(Type type);
    }
}