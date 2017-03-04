namespace LogViewer.Library.Module.Services.RandomLog
{
    public class RandomLogItemConnection : IRandomLogItemConnection
    {
        private string _value;
        
        public string Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
            }
        }
    }
}