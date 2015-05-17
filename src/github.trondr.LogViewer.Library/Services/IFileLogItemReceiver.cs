namespace github.trondr.LogViewer.Library.Services
{
    public interface IFileLogItemReceiver : ILogItemReceiver
    {
        string LogFileName { get; set; }
        bool ShowFromBegining { get; set; }
    }
}