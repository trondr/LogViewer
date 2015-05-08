namespace github.trondr.LogViewer.Library.Commands.Example
{
    public interface IOpenLogCommandProviderFactory
    {
        IOpenLogCommandProvider GetOpenLogCommandProvider();

        void Release(IOpenLogCommandProvider openLogCommandProvider);
    }
}