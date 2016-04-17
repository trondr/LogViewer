using System.Linq;
using Common.Logging;
using Common.Logging.Simple;
using github.trondr.LogViewer.Infrastructure;
using NUnit.Framework;
using github.trondr.LogViewer.Library.Module.Services;

namespace github.trondr.LogViewer.Tests.UnitTests
{
    [TestFixture(Category = "UnitTests")]
    public class LogItemConnectionProviderTests
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

        [Test]
        public void UnsupportedConnectionStringThrowUnSupportedConnectionStringException()
        {
            using(var testBooStrapper = new BootStrapper())
            {
                var target = testBooStrapper.Container.Resolve<ILogItemConnectionProvider>();                    
                Assert.Throws<UnSupportedConnectionStringException>(() => {var actual = target.GetLogItemConnections(new []{"some:un:supported:connection:string"}).ToList();});
            }
        }

        [Test]
        public void DuplicateConnectionStringThrowDuplicateConnectionStringException()
        {
            using(var testBooStrapper = new BootStrapper())
            {
                var target = testBooStrapper.Container.Resolve<ILogItemConnectionProvider>(); 
                Assert.Throws<DuplicateConnectionStringException>(() => {var actual = target.GetLogItemConnections(new []{"tcp:somehost:8099:ipv4","tcp:somehost:8099:ipv4"}).ToList();});
            }
        }
    }
}