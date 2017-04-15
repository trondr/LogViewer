namespace LogViewer.Library.Module.Services
{
    public interface ISourceCodeInfoProvider
    {
        SourceCodeInfo GetSourceCode(string fileName);
    }
}
