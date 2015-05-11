using System.Collections.Generic;
using github.trondr.LogViewer.Library.ViewModels;

namespace github.trondr.LogViewer.Library.Services
{
    public interface ILogItemService
    {
        IEnumerable<LogItemViewModel> GetLogs(); 
    }    
}                                                                                                                                                
