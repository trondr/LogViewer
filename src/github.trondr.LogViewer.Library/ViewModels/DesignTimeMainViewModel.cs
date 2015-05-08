using System.Windows.Input;
using github.trondr.LogViewer.Library.Common.UI;

namespace github.trondr.LogViewer.Library.ViewModels
{
    public class DesignTimeMainViewModel : ViewModelBase, IMainViewModel
    {
        public DesignTimeMainViewModel()
        {
            this.ProductDescription = "My Product Description";
            this.ProductDescriptionLabelText = "Product Description:";
            this.MaxLabelWidth = 200;
        }

        public int MaxLabelWidth { get; set; }
        public string ProductDescription { get; set; }
        public string ProductDescriptionLabelText { get; set; }
        public ICommand OkCommand { get; set; }

    }
}