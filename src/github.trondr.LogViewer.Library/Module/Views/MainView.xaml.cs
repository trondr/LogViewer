using github.trondr.LogViewer.Library.Module.Common.UI;
using github.trondr.LogViewer.Library.Module.ViewModels;

namespace github.trondr.LogViewer.Library.Module.Views
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : ViewBase
    {
        public MainView(MainViewModel viewModel)
        {
            this.ViewModel = viewModel;
            InitializeComponent();
        }
    }
}
