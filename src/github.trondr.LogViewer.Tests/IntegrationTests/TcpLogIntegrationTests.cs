using System;
using System.Threading;
using Common.Logging;
using Common.Logging.Simple;
using github.trondr.LogViewer.Infrastructure;
using github.trondr.LogViewer.Library.Module.Commands.OpenLog;
using NUnit.Framework;

namespace github.trondr.LogViewer.Tests.IntegrationTests
{
    [TestFixture(Category = "IntegrationTests")]
    public class TcpLogIntegrationTests
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
        public void TcpLogTest()
        {
            StartTcpTestLogger();
            using (var bootStrapper = new BootStrapper())
            {
                var container = bootStrapper.Container;
                var openLogProvider = container.Resolve<IOpenLogCommandProvider>();
                openLogProvider.OpenLogs(new string[] {"tcp:somehost:8099:ipv4"});
            }            
        }

        private void StartTcpTestLogger()
        {
            var threadStart = new ThreadStart(Start);
            var thread = new Thread(threadStart);
            thread.Start();
        }

        private void Start()
        {
            try
            {
                var logGenerator = new TestTcpLogGenerator("localhost",8099);
                logGenerator.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            
        }
    }
}