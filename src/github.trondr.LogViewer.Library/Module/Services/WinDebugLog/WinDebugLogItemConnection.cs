namespace github.trondr.LogViewer.Library.Module.Services.WinDebugLog
{
    public class WinDebugLogItemConnection : IWinDebugLogItemConnection
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