using LogViewer.Library.Infrastructure.LifeStyles;

namespace LogViewer.Library.Module.Services
{
    [Singleton]
    public class LogViewerConfiguration: ILogViewerConfiguration
    {
        private string[] _connectionStrings;

        public string[] ConnectionStrings
        {
            get => _connectionStrings ?? (_connectionStrings = new string[]{});
            set => _connectionStrings = value;
        }
    }
}