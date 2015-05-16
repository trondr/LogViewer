using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace github.trondr.LogViewer.Library.ViewModels
{
    public class LoggerViewModel: DependencyObject
    {
        public LoggerViewModel(string logger)
        {
            Name = logger;
            var index = logger.LastIndexOf(".");
            if(index > 0)
            {
                DisplayName = logger.Substring(index + 1, logger.Length - index - 1);
            }
            else
            {
                DisplayName = Name;
            }
            Children = new ObservableCollection<LoggerViewModel>();
            IsVisible = true;
        }

        public static readonly DependencyProperty NameProperty = DependencyProperty.Register(
            "Name", typeof (string), typeof (LoggerViewModel), new PropertyMetadata(default(string)));

        public string Name
        {
            get { return (string) GetValue(NameProperty); }
            set { SetValue(NameProperty, value); }
        }
        
        public static readonly DependencyProperty DisplayNameProperty = DependencyProperty.Register(
            "DisplayName", typeof (string), typeof (LoggerViewModel), new PropertyMetadata(default(string)));

        public string DisplayName
        {
            get { return (string) GetValue(DisplayNameProperty); }
            set { SetValue(DisplayNameProperty, value); }
        }

        public static readonly DependencyProperty ParentProperty = DependencyProperty.Register(
            "Parent", typeof (LoggerViewModel), typeof (LoggerViewModel), new PropertyMetadata(default(LoggerViewModel)));

        public LoggerViewModel Parent
        {
            get { return (LoggerViewModel) GetValue(ParentProperty); }
            set { SetValue(ParentProperty, value); }
        }

        public ObservableCollection<LoggerViewModel> Children { get; set; }

        public static readonly DependencyProperty IsVisibleProperty = DependencyProperty.Register(
            "IsVisible", typeof (bool), typeof (LoggerViewModel), new FrameworkPropertyMetadata(default(bool),PropertyChangedCallback));

        public bool IsVisible
        {
            get { return (bool) GetValue(IsVisibleProperty); }
            set { SetValue(IsVisibleProperty, value); }
        }

        private static void PropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var loggerViewModel = (LoggerViewModel)dependencyObject;
            if ((bool)dependencyPropertyChangedEventArgs.NewValue != (bool)dependencyPropertyChangedEventArgs.OldValue)
            {                
                loggerViewModel.Children.Select(model => model.IsVisible = (bool)dependencyPropertyChangedEventArgs.NewValue).ToList();
            }
 
        }
    }
}