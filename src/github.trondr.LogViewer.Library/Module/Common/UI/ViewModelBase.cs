using System;
using System.Windows;
using github.trondr.LogViewer.Library.Module.Views;

namespace github.trondr.LogViewer.Library.Module.Common.UI
{
    public abstract class ViewModelBase : DependencyObject
    {
        public MainWindow MainWindow { get; set; }        
    }
}
