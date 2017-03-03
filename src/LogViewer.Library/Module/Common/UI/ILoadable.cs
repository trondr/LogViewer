using System.Threading.Tasks;

namespace LogViewer.Library.Module.Common.UI
{
    public interface ILoadable
    {        
        Task LoadAsync();
        
        Task UnloadAsync();

        LoadStatus LoadStatus { get; set; }
    }
}