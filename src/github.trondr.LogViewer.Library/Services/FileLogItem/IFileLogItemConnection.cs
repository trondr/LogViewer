namespace github.trondr.LogViewer.Library.Services.FileLogItem
{
    public interface IFileLogItemConnection :ILogItemConnection
    {
        string FileName { get; set; }
    }
}