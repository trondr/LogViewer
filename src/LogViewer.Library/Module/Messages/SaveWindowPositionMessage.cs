using GalaSoft.MvvmLight.Messaging;

namespace LogViewer.Library.Module.Messages
{
    public class SaveWindowPositionMessage : MessageBase
    {
        public string WindowPlacement { get; set; }
    }
}