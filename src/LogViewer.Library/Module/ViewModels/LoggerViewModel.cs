using System;
using System.Collections.ObjectModel;
using System.Linq;
using GalaSoft.MvvmLight;
using LogViewer.Library.Module.Common.UI;

namespace LogViewer.Library.Module.ViewModels
{
    public class LoggerViewModel : ViewModelBase, ILoggerViewModel
    {
        private ObservableCollection<LoggerViewModel> _children;
        private string _displayName;
        private bool _isVisible;
        private string _name;
        private LoggerViewModel _parent;

        public LoggerViewModel(string logger)
        {
            Name = logger;
            var index = logger.LastIndexOf(".", StringComparison.Ordinal);
            DisplayName = index > 0 ? logger.Substring(index + 1, logger.Length - index - 1) : Name;            
            IsVisible = true;
        }

        public ObservableCollection<LoggerViewModel> Children
        {
            get { return _children ?? (Children = new ObservableCollection<LoggerViewModel>()); }
            set { this.SetProperty(ref _children, value); }
        }

        public string DisplayName
        {
            get { return _displayName; }
            set { this.SetProperty(ref _displayName, value); }
        }

        public bool IsVisible
        {
            get { return _isVisible; }
            set { this.SetProperty(ref _isVisible, value, () =>
            {                
                Children.ToList().ForEach(model => model.IsVisible = value);
            }); }
        }
        
        public string Name
        {
            get { return _name; }
            set { this.SetProperty(ref _name, value); }
        }

        public LoggerViewModel Parent
        {
            get { return _parent; }
            set { this.SetProperty(ref _parent, value); }
        }
    }
}