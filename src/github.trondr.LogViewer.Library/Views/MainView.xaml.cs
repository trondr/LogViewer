using github.trondr.LogViewer.Library.Common.UI;
using github.trondr.LogViewer.Library.ViewModels;

namespace github.trondr.LogViewer.Library.Views
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
