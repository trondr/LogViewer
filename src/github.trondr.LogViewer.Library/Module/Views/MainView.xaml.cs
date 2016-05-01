using System.Windows.Controls;
using System.Windows.Media;
using github.trondr.LogViewer.Library.Module.ViewModels;
using ViewBase = github.trondr.LogViewer.Library.Module.Common.UI.ViewBase;

namespace github.trondr.LogViewer.Library.Module.Views
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : ViewBase
    {
        public MainView(MainViewModel viewModel)
        {
            viewModel.ScrollToBottom += ScrollToBottom;
            this.ViewModel = viewModel;            
            InitializeComponent();
        }

        private void ScrollToBottom()
        {
            if (VisualTreeHelper.GetChildrenCount(_logItemsListView) > 0)
            {
                var listBoxChrome = VisualTreeHelper.GetChild(_logItemsListView, 0);
                var scrollViewer = VisualTreeHelper.GetChild(listBoxChrome, 0) as ScrollViewer;
                if(scrollViewer != null)
                {                 
                    scrollViewer.ScrollToBottom();
                    _logItemsListView.SelectedIndex = _logItemsListView.Items.Count;
                }                
            }
        }
    }
}
