namespace LogViewer.Library.Infrastructure
{
    public interface ITypeMapper
    {
        T Map<T>(object source);
    }
}
