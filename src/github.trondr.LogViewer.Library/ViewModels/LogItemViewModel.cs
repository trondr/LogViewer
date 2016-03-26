using System;
using System.Windows;

namespace github.trondr.LogViewer.Library.ViewModels
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
            "ThreadId", typeof (int), typeof (LogItemViewModel), new PropertyMetadata(default(int)));

        public int ThreadId
        {
            get { return (int) GetValue(ThreadIdProperty); }
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
    }
}