namespace github.trondr.LogViewer.Library.Services.FileLogItem
{
    public class FileLogItemConnection : IFileLogItemConnection
    {
        private string _value;

        public FileLogItemConnection(string value)
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