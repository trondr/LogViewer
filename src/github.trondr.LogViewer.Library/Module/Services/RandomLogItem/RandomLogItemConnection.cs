namespace github.trondr.LogViewer.Library.Module.Services.RandomLogItem
{
    public class RandomLogItemConnection : IRandomLogItemConnection
    {
        private string _value;

        public RandomLogItemConnection(string value)
        {
            Value = value;
        }

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