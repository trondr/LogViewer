namespace github.trondr.LogViewer.Library.Module.Services.FileLog
{
    public interface IFileLogItemConnection :ILogItemConnection
    {
        string FileName { get; set; }
    }
}