using System;
using System.Windows;
using LogViewer.Library.Module.Common.Collection;

namespace LogViewer.Library.Module.ViewModels
{
    
    public class LogItemViewModel : DependencyObject
    {
        public static readonly DependencyProperty TimeProperty = DependencyProperty.Register(
            "Time", typeof (DateTime), typeof (LogItemViewModel), new PropertyMetadata(default(DateTime)));

        public DateTime Time
        {
            get { return (DateTime) GetValue(TimeProperty); }
            set { SetValue(TimeProperty, value); }
        }

        public static readonly DependencyProperty LogLevelProperty = DependencyProperty.Register(
            "LogLevel", typeof (LogLevelViewModel), typeof (LogItemViewModel), new PropertyMetadata(default(LogLevelViewModel)));

        public LogLevelViewModel LogLevel
        {
            get { return (LogLevelViewModel) GetValue(LogLevelProperty); }
            set { SetValue(LogLevelProperty, value); }
        }

        public static readonly DependencyProperty LoggerProperty = DependencyProperty.Register(
            "Logger", typeof (LoggerViewModel), typeof (LogItemViewModel), new PropertyMetadata(default(LoggerViewModel)));

        public LoggerViewModel Logger
        {
            get { return (LoggerViewModel) GetValue(LoggerProperty); }
            set { SetValue(LoggerProperty, value); }
        }

        public static readonly DependencyProperty ThreadIdProperty = DependencyProperty.Register(
            "ThreadId", typeof (string), typeof (LogItemViewModel), new PropertyMetadata(default(string)));

        public string ThreadId
        {
            get { return (string) GetValue(ThreadIdProperty); }
            set { SetValue(ThreadIdProperty, value); }
        }

        public static readonly DependencyProperty MessageProperty = DependencyProperty.Register(
            "Message", typeof (string), typeof (LogItemViewModel), new PropertyMetadata(default(string)));

        public string Message
        {
            get { return (string) GetValue(MessageProperty); }
            set { SetValue(MessageProperty, value); }
        }

        public static readonly DependencyProperty IsVisibleProperty = DependencyProperty.Register(
            "IsVisible", typeof (bool), typeof (LogItemViewModel), new PropertyMetadata(default(bool)));

        public bool IsVisible
        {
            get { return (bool) GetValue(IsVisibleProperty); }
            set { SetValue(IsVisibleProperty, value); }
        }
        
        public ObservableDictionary<string, string> Properties { get; set; }

        public static readonly DependencyProperty ExceptionStringProperty = DependencyProperty.Register(
            "ExceptionString", typeof (string), typeof (LogItemViewModel), new PropertyMetadata(default(string)));

        public string ExceptionString
        {
            get { return (string) GetValue(ExceptionStringProperty); }
            set { SetValue(ExceptionStringProperty, value); }
        }
    }
}