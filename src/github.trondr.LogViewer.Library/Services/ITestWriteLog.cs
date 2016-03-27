using Xceed.Wpf.DataGrid.FilterCriteria;

namespace github.trondr.LogViewer.Library.Services
{
    public interface ITestWriteLog
    {
        void StartWritingLog();

        void StopWritingLog();
    }
}