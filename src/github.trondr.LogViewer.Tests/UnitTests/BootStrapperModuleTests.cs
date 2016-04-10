using Castle.Facilities.TypedFactory;
using Common.Logging;
using Common.Logging.Simple;
using github.trondr.LogViewer.Infrastructure;
using github.trondr.LogViewer.Library.Module.Services;
using github.trondr.LogViewer.Library.Module.Services.EventLogItem;
using github.trondr.LogViewer.Library.Module.Services.FileLogItem;
using github.trondr.LogViewer.Library.Module.Services.RandomLogItem;
using github.trondr.LogViewer.Library.Module.ViewModels;
using github.trondr.LogViewer.Tests.Common;
using NUnit.Framework;
using Rhino.Mocks.Constraints;

namespace github.trondr.LogViewer.Tests.UnitTests
{
    [TestFixture(Category = "UnitTests")]
    public class BootStrapperModuleTests
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
        public void Log4JLogItemParserRegistrationTest()
        {
            BootStrapperTestsHelper.CheckThatNumberOfResolvedServicesAre<ILog4JLogItemParser>(1);
            BootStrapperTestsHelper.CheckThatResolvedServiceIsOfInstanceType<ILog4JLogItemParser, Log4JLogItemParser>();
            BootStrapperTestsHelper.CheckThatResolvedServiceHasSingletonLifeCycle<ILog4JLogItemParser>();
        }

        [Test, RequiresSTA]
        public void LoggerViewModelProviderRegistrationTest()
        {
            BootStrapperTestsHelper.CheckThatNumberOfResolvedServicesAre<ILoggerViewModelProvider>(1);
            BootStrapperTestsHelper.CheckThatResolvedServiceIsOfInstanceType<ILoggerViewModelProvider, LoggerViewModelProvider>();
            BootStrapperTestsHelper.CheckThatResolvedServiceHasSingletonLifeCycle<ILoggerViewModelProvider>();
        }

        [Test, RequiresSTA]
        public void LogLevelViewModelProviderRegistrationTest()
        {
            BootStrapperTestsHelper.CheckThatNumberOfResolvedServicesAre<ILogLevelViewModelProvider>(1);
            BootStrapperTestsHelper.CheckThatResolvedServiceIsOfInstanceType<ILogLevelViewModelProvider, LogLevelViewModelProvider>();
            BootStrapperTestsHelper.CheckThatResolvedServiceHasSingletonLifeCycle<ILogLevelViewModelProvider>();
        }        

        [Test, RequiresSTA]
        public void ILogItemHandlerFactoryRegistrationTest()
        {
            BootStrapperTestsHelper.CheckThatNumberOfResolvedServicesAre<ILogItemHandlerFactory>(1);
            BootStrapperTestsHelper.CheckThatResolvedServiceHasSingletonLifeCycle<ILogItemHandlerFactory>();
            using (var bootStrapper = new BootStrapperInstance())
            {
                var target = bootStrapper.Container.Resolve<ILogItemHandlerFactory>();
                var logItemHandler = target.GetLogItemHandlers(new FileLogItemConnection());
                Assert.AreEqual(1, logItemHandler.Length, "Handlers for file log item connection is not 1.");
                Assert.AreEqual(typeof(FileLogItemHandler), logItemHandler[0].GetType(), "Log item handler is not of type FileLogItemHandler");

                logItemHandler = target.GetLogItemHandlers(new EventLogItemConnection());
                Assert.AreEqual(1, logItemHandler.Length, "Handlers for event log item connection is not 1.");
                Assert.AreEqual(typeof(EventLogItemHandler), logItemHandler[0].GetType(), "Log item handler is not of type EventLogItemHandler");

                logItemHandler = target.GetLogItemHandlers(new RandomLogItemConnection());
                Assert.AreEqual(1, logItemHandler.Length, "Handlers for event log item connection is not 1.");
                Assert.AreEqual(typeof(RandomLogItemHandler), logItemHandler[0].GetType(), "Log item handler is not of type RandomLogItemHandler");
            }            
        }
    }
}