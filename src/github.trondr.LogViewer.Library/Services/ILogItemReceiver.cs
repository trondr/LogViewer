using LogLevel = Common.Logging.LogLevel;

namespace github.trondr.LogViewer.Library.Services
{
    public interface ILogItemReceiver
    {
        void Initialize();

        void Terminate();

        void Attach(ILogItemNotifiable logItemNotifiable);

        void Detach();

        string DefaultLoggerName { get; set; }
    }
}