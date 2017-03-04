using System.Timers;
using Common.Logging;

namespace LogViewer.Library.Module.Services
{
    public class TestWriteLog : ITestWriteLog
    {
        private readonly ILog _logger;
        private Timer _timer;

        public TestWriteLog(ILog logger)
        {
            _logger = logger;
        }

        private void TimerOnElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            _logger.Info("Testing Info");
            _logger.Error("Testing Error");
            _logger.Warn("Testing Warn");
        }

        public void StartWritingLog()
        {
            StopWritingLog();
            _timer = new Timer(10);     
            _timer.Elapsed+= TimerOnElapsed;
            _timer.Start();
        }

        public void StopWritingLog()
        {
            if(_timer != null)
            {
                _timer.Stop();
                _timer.Close();
                _timer.Dispose();
                _timer = null;
            }
        }
    }
}