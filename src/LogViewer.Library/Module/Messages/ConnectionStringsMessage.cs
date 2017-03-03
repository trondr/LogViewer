using GalaSoft.MvvmLight.Messaging;

namespace LogViewer.Library.Module.Messages
{
    public class ConnectionStringsMessage: MessageBase
    {
        public string[] ConnectionStrings { get; set; }
    }
}