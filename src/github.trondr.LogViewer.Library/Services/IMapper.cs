namespace github.trondr.LogViewer.Library.Services
{
    public interface IMapper
    {
        T Map<T>(object source);
    }
}
