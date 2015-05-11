namespace github.trondr.LogViewer.Library.Commands.OpenLog
{
    public interface IOpenLogCommandProviderFactory
    {
        IOpenLogCommandProvider GetOpenLogCommandProvider();

        void Release(IOpenLogCommandProvider openLogCommandProvider);
    }
}