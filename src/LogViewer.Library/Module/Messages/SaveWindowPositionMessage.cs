using GalaSoft.MvvmLight.Messaging;

namespace LogViewer.Library.Module.Messages
{
    public class SaveWindowPositionMessage : MessageBase
    {
        public WindowPosition Position { get; set; } = new WindowPosition();
    }
}