using GalaSoft.MvvmLight.Messaging;

namespace LogViewer.Library.Module.Messages
{
    public class SetWindowPositionMessage : MessageBase
    {
        public string WindowPlacement { get; set; } = string.Empty;
    }
}