namespace github.trondr.LogViewer.Library.Module.Services
{
    public interface ILogItemConnectionStringParser
    {
        bool CanParse(string connectionString);

        ILogItemConnection Parse(string connectionString);
    }
}