namespace github.trondr.LogViewer.Library.Module.Services.EventLogItem
{
    public class EventLogItemHandler : ILogItemHandler<EventLogItemConnection>
    {
        public void Initialize()
        {
            throw new System.NotImplementedException();
        }

        public void Terminate()
        {
            throw new System.NotImplementedException();
        }

        public void Attach(ILogItemNotifiable logItemNotifiable)
        {
            throw new System.NotImplementedException();
        }

        public void Detach()
        {
            throw new System.NotImplementedException();
        }

        public bool ShowFromBeginning { get; set; }

        public ILogItemConnection Connection { get; set; }
    }
}