using System;
using System.Threading;
using Common.Logging;
using Common.Logging.Simple;
using github.trondr.LogViewer.Infrastructure;
using github.trondr.LogViewer.Library.Module.Commands.OpenLog;
using NUnit.Framework;

namespace github.trondr.LogViewer.Tests.ManualTests.UdpLogTests
{
    [TestFixture(Category = "ManualTests")]
    public class UdpLogManualTests
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
        public void UdpLogTest()
        {
            using (var bootStrapper = new BootStrapper())
            {
                var container = bootStrapper.Container;
                var openLogProvider = container.Resolve<IOpenLogCommandProvider>();
                var logGenerator = new TestUdpLogGenerator("localhost", 8099);
                StartTcpTestLogger(logGenerator);
                var exitCode = openLogProvider.OpenLogs(new string[] {"udp:somehost:8099:ipv4"});
                Assert.AreEqual(0, exitCode, "OpenLog exited with error");
                StoppTcpTestLogger(logGenerator);
            }
        }

        private static void StoppTcpTestLogger(TestUdpLogGenerator logGenerator)
        {
            logGenerator.Stop();
            Thread.Sleep(1000);
        }

        private void StartTcpTestLogger(TestUdpLogGenerator logGenerator)
        {
            if (logGenerator == null) throw new ArgumentNullException(nameof(logGenerator));
            var threadStart = new ThreadStart(logGenerator.Start);
            var thread = new Thread(threadStart);
            thread.Start();
        }        
    }
}