namespace github.trondr.LogViewer.Library.Module.Services.FileLogItem
{
    public interface IFileLogItemConnection :ILogItemConnection
    {
        string FileName { get; set; }
    }
}