namespace github.trondr.LogViewer.Library.Commands.OpenLog
{
    public interface IOpenLogCommandProvider
    {
        int OpenLogs(string[] connectionStrings);
    }
}
