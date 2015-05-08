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

        public static readonly DependencyProperty LevelProperty = DependencyProperty.Register(
            "Level", typeof (LogLevelViewModel), typeof (LogItemViewModel), new PropertyMetadata(default(LogLevelViewModel)));

        public LogLevelViewModel Level
        {
            get { return (LogLevelViewModel) GetValue(LevelProperty); }
            set { SetValue(LevelProperty, value); }
        }

        public static readonly DependencyProperty LoggerProperty = DependencyProperty.Register(
            "Logger", typeof (string), typeof (LogItemViewModel), new PropertyMetadata(default(string)));

        public string Logger
        {
            get { return (string) GetValue(LoggerProperty); }
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
    }
}