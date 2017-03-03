using LogViewer.Library.Module.Model;

namespace LogViewer.Library.Module.Services
{
    /// <summary>
    /// Source: http://log2console.codeplex.com/SourceControl/latest#src/Log2Console/Log/ILogMessageNotifiable.cs
    /// </summary>
    public interface ILogItemNotifiable
    {
        void Notify(LogItem[] logItems);

        void Notify(LogItem logItem);
    }
}