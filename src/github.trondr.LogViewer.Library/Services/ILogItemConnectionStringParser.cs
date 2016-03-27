namespace github.trondr.LogViewer.Library.Services
{
    public interface ILogItemConnectionStringParser
    {
        bool CanParse(string connectionString);

        ILogItemConnection Parse(string connectionString);
    }
}