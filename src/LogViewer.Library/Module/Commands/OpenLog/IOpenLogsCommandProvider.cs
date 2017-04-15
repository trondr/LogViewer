namespace LogViewer.Library.Module.Commands.OpenLog
{
    public interface IOpenLogsCommandProvider
    {
        int OpenLogs(string[] connectionStrings);
    }
}
