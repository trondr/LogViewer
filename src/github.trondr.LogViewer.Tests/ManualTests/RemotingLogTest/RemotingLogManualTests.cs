using System;
using System.Threading;
using Common.Logging;
using Common.Logging.Simple;
using github.trondr.LogViewer.Infrastructure;
using github.trondr.LogViewer.Library.Module.Commands.OpenLog;
using github.trondr.LogViewer.Tests.ManualTests.UdpLogTests;
using NUnit.Framework;

namespace github.trondr.LogViewer.Tests.ManualTests.RemotingLogTest
{
    [TestFixture(Category = "ManualTests")]
    public class RemotingLogManualTests
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
        public void RemotingLogTest()
        {
            using (var bootStrapper = new BootStrapper())
            {
                var container = bootStrapper.Container;
                var openLogProvider = container.Resolve<IOpenLogCommandProvider>();
                var logGenerator = new TestRemotingLogGenerator("tcp://localhost:7070/LoggingSink");
                StartTestLogger(logGenerator);
                var exitCode = openLogProvider.OpenLogs(new string[] {"remoting:LoggingSink:7070"});
                Assert.AreEqual(0, exitCode, "OpenLog exited with error");
                StopTestLogger(logGenerator);
            }
        }

        private static void StopTestLogger(TestRemotingLogGenerator logGenerator)
        {
            logGenerator.Stop();
            Thread.Sleep(1000);
        }

        private void StartTestLogger(TestRemotingLogGenerator logGenerator)
        {
            if (logGenerator == null) throw new ArgumentNullException(nameof(logGenerator));
            var threadStart = new ThreadStart(logGenerator.Start);
            var thread = new Thread(threadStart);
            thread.Start();
        }        
    }
}