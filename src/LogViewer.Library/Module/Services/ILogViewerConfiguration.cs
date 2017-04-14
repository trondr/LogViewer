namespace LogViewer.Library.Module.Services
{
    public interface ILogViewerConfiguration
    {
        string[] ConnectionStrings { get; set; }
    }
}
