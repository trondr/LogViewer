using System;
using System.Threading;
using Common.Logging;
using Common.Logging.Simple;
using github.trondr.LogViewer.Infrastructure;
using github.trondr.LogViewer.Library.Module.Commands.OpenLog;
using NUnit.Framework;

namespace github.trondr.LogViewer.Tests.ManualTests.FileLogTests
{
    [TestFixture(Category = "ManualTests")]
    public class FileLogManualTests
    {
        private ConsoleOutLogger _logger;

        [SetUp]
        public void SetUp()
        {
            _logger = new ConsoleOutLogger(this.GetType().Name, LogLevel.All, true, false, false, "yyyy-MM-dd hh:mm:ss");            
        }

        [TearDown]
        public void TearDown()
        {

        }

        [Test, RequiresSTA]
        public void FileLogTest()
        {
            using (var bootStrapper = new BootStrapper())
            {
                var container = bootStrapper.Container;
                var openLogProvider = container.Resolve<IOpenLogCommandProvider>();
                var logGenerator = new TestFileLogGenerator(@"%public%\Logs\FileLogTests-%COMPUTERNAME%-%USERNAME%.log");
                StartFileTestLogger(logGenerator);
                openLogProvider.OpenLogs(new string[] {@"file://%public%\Logs\FileLogTests-%COMPUTERNAME%-%USERNAME%.log"});
                StoppTcpTestLogger(logGenerator);
            }
        }

        private static void StoppTcpTestLogger(TestFileLogGenerator logGenerator)
        {
            logGenerator.Stop();
            Thread.Sleep(1000);
        }

        private void StartFileTestLogger(TestFileLogGenerator logGenerator)
        {
            if (logGenerator == null) throw new ArgumentNullException(nameof(logGenerator));
            var threadStart = new ThreadStart(logGenerator.Start);
            var thread = new Thread(threadStart);
            thread.Start();
        }        
    }
}