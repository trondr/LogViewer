using GalaSoft.MvvmLight;
using LogViewer.Library.Module.Common.UI;

namespace LogViewer.Library.Module.Services
{
    public class SourceCodeInfo: ViewModelBase
    {
        private string _code;

        public string Code  
        {
            get { return _code; }
            set { this.SetProperty(ref _code, value); }
        }
    }
}