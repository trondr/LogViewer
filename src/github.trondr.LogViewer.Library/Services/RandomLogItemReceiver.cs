using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using github.trondr.LogViewer.Library.Model;

namespace github.trondr.LogViewer.Library.Services
{
    public class RandomLogItemReceiver : IRandomLogItemReceiver
    {
        private Timer _timer    ;
        private ILogItemNotifiable _logItemNotifiable;
        private static readonly List<LogLevel> LogLevels = Enum.GetValues(typeof(LogLevel)).Cast<LogLevel>().ToList();
        private static Random _random = new Random();

        public void Initialize()
        {
            _timer?.Dispose();
            _timer = new Timer(1000);
            _timer.Elapsed += TimerElapsed;
            _timer.Start();

        }

        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            PublishLogItems();
        }

        private void PublishLogItems()
        {
            var logItems = GetRandomLogItems();
            NotifyUserIterface(logItems);
        }

        private void NotifyUserIterface(IEnumerable<LogItem> logItems)
        {
            _logItemNotifiable?.Notify(logItems.ToArray());
        }

        private static IEnumerable<LogItem> GetRandomLogItems()
        {
            for (var i = 0; i < GetRandomMax(); i++)
            {
                yield return GetRandomLogItem();
            }
        }

        private static int GetRandomMax()
        {
            return _random.Next(1,10);
        }

        private static LogItem GetRandomLogItem()
        {
            var logItem = new LogItem
            {
                LogLevel = GetRandomLogLevel(),
                Logger = GetRandomLogger(),
                Message = GetRandomMessage(),
                ThreadId = GetRandomThreadId(),
                Time = DateTime.Now
            };
            return  logItem;
        }

        private static readonly string[] _threadIds = new []{"1","2","3","4","5","6","7","8","9","10"};

        private static string GetRandomThreadId()
        {
            return _threadIds[_random.Next(0, 9)];
        }

        private static readonly string[] _messages = new []{"Message 1","Message 2","Message 3","Message 4","Message 5","Message 6","Message 7","Message 8","Message 9","Message 10"};

        private static string GetRandomMessage()
        {
            return _messages[_random.Next(0, 9)];
        }

        private static readonly string[] _loggers = new []{"Company.Product.Class1","Company.Product.Class2","Company.Product.Class3","Company.Product.Class4","Company.Product.Class5","Company.Product.Class1.SubClass1","Company.Product.Class2.SubClass2","Company.Product.Class3.SubClass3","Company.Product.Class4.SubClass4","Company.Product.Class5.SubClass5"};

        private static string GetRandomLogger()
        {
            return _loggers[_random.Next(0, 9)];
        }

        private static LogLevel GetRandomLogLevel()
        {                        
            return LogLevels[_random.Next(0, LogLevels.Count - 1)];
        }

        public void Terminate()
        {
            _timer?.Stop();
            _timer?.Dispose();
        }

        public void Attach(ILogItemNotifiable logItemNotifiable)
        {
            _logItemNotifiable = logItemNotifiable;
        }

        public void Detach()
        {
            _logItemNotifiable = null;
        }

        public string DefaultLoggerName { get; set; }
    }
}