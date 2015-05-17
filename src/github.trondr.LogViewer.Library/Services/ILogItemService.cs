using System.Collections.Generic;
using github.trondr.LogViewer.Library.ViewModels;

namespace github.trondr.LogViewer.Library.Services
{
    public interface ILogItemService
    {
        IEnumerable<LogItemViewModel> GetLogs(); 
    }

    //public class FileLogItemService : ILogItemService
    //{
        
    //    public IEnumerable<LogItemViewModel> GetLogs()
    //    {
    //        throw new System.NotImplementedException();
    //    }
    //}
}                                                                                                                                                
