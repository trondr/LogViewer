using System.Collections;
using System.Collections.Generic;
using github.trondr.LogViewer.Library.Model;

namespace github.trondr.LogViewer.Library.Services
{
    public interface IFileLogItemReceiver : ILogItemReceiver
    {
        string LogFileName { get; set; }
        bool ShowFromBeginning { get; set; }
    }
}