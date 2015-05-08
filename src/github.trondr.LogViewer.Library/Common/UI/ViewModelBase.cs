using System.Windows;
using github.trondr.LogViewer.Library.Views;

namespace github.trondr.LogViewer.Library.Common.UI
{
    public abstract class ViewModelBase : DependencyObject
    {
        public MainWindow MainWindow { get; set; }
    }
}
