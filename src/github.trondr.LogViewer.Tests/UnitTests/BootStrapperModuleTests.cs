using System;
using System.Collections.Generic;
using Common.Logging.Simple;
using github.trondr.LogViewer.Infrastructure;
using github.trondr.LogViewer.Library.Infrastructure;
using github.trondr.LogViewer.Library.Module.Model;
using github.trondr.LogViewer.Library.Module.Services;
using github.trondr.LogViewer.Library.Module.Services.EventLog;
using github.trondr.LogViewer.Library.Module.Services.FileLog;
using github.trondr.LogViewer.Library.Module.Services.RandomLog;
using github.trondr.LogViewer.Library.Module.ViewModels;
using github.trondr.LogViewer.Tests.Common;
using NUnit.Framework;
using LogLevel = Common.Logging.LogLevel;

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
        }

        [Test, RequiresSTA]
        public void FileLogItemConnectionFactoryRegistrationTest()
        {
            BootStrapperTestsHelper.CheckThatNumberOfResolvedServicesAre<IFileLogItemConnectionFactory>(1);            
            BootStrapperTestsHelper.CheckThatResolvedServiceHasSingletonLifeCycle<IFileLogItemConnectionFactory>();                
        }

        [Test, RequiresSTA]
        public void EventLogItemConnectionFactoryRegistrationTest()
        {
            BootStrapperTestsHelper.CheckThatNumberOfResolvedServicesAre<IEventLogItemConnectionFactory>(1);            
            BootStrapperTestsHelper.CheckThatResolvedServiceHasSingletonLifeCycle<IEventLogItemConnectionFactory>();                
        }

        [Test, RequiresSTA]
        public void RandomLogItemConnectionFactoryRegistrationTest()
        {
            BootStrapperTestsHelper.CheckThatNumberOfResolvedServicesAre<IRandomLogItemConnectionFactory>(1);            
            BootStrapperTestsHelper.CheckThatResolvedServiceHasSingletonLifeCycle<IRandomLogItemConnectionFactory>();                
        }
        
        [Test, RequiresSTA]
        public void FileLogItemHandlerFactoryRegistrationTest()
        {
            using (var bootStrapper = new BootStrapper())
            {
                var target = bootStrapper.Container.Resolve<ILogItemHandlerFactory>();
                var logItemHandler = target.GetLogItemHandlers(new FileLogItemConnection());
                Assert.AreEqual(1, logItemHandler.Length, "Handlers for file log item connection is not 1.");
                Assert.AreEqual(typeof(FileLogItemHandler), logItemHandler[0].GetType(), "Log item handler is not of type FileLogItemHandler");
            }            
        }

        [Test, RequiresSTA]
        public void EventLogItemHandlerFactoryRegistrationTest()
        {
            using (var bootStrapper = new BootStrapper())
            {
                var target = bootStrapper.Container.Resolve<ILogItemHandlerFactory>();
                var logItemHandler = target.GetLogItemHandlers(new EventLogItemConnection());
                Assert.AreEqual(1, logItemHandler.Length, "Handlers for event log item connection is not 1.");
                Assert.AreEqual(typeof(EventLogItemHandler), logItemHandler[0].GetType(), "Log item handler is not of type EventLogItemHandler");
            }            
        }

        [Test, RequiresSTA]
        public void RandomLogItemHandlerFactoryRegistrationTest()
        {
            using (var bootStrapper = new BootStrapper())
            {
                var target = bootStrapper.Container.Resolve<ILogItemHandlerFactory>();
                var logItemHandler = target.GetLogItemHandlers(new RandomLogItemConnection());
                Assert.AreEqual(1, logItemHandler.Length, "Handlers for Random log item connection is not 1.");
                Assert.AreEqual(typeof(RandomLogItemHandler), logItemHandler[0].GetType(), "Log item handler is not of type RandomLogItemHandler");
            }            
        }        
    }

    [TestFixture(Category = "UnitTests")]
    public class AutoMapperConfigurationTests
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
        public void LogItemMapperConfigurationTest()
        {
            using (var bootStrapper = new BootStrapper())
            {
                var typeMapper = bootStrapper.Container.Resolve<ITypeMapper>();
                var messsage = "An error";
                var testLog = "test.log";
                var threadId = "1";
                var someExceptionString = "Some exception string";
                var key1 = "key1";
                var key2 = "key2";
                var value1 = "value1";
                var value2 = "value2";
                var testLogItem = new LogItem()
                {
                    Logger = testLog,
                    Message = messsage,
                    ThreadId = threadId,
                    Time = DateTime.Now,
                    ExceptionString = someExceptionString,
                    LogLevel = Library.Module.Model.LogLevel.Debug,
                    Properties = new Dictionary<string, string>
                {
                    { key1,value1},
                    { key2,value2},
                }
                };
                var logItemViewModel = typeMapper.Map<LogItemViewModel>(testLogItem);                
                Assert.AreEqual(Library.Module.Model.LogLevel.Debug,logItemViewModel.LogLevel.Level,"LogLevel is not correct");
                Assert.AreEqual(testLog,logItemViewModel.Logger.Name,"Logger is not correct");
                Assert.AreEqual(messsage,logItemViewModel.Message,"Message is not correct");
                Assert.AreEqual(someExceptionString,logItemViewModel.ExceptionString,"ExceptionString is not correct");
                CollectionAssert.AreEquivalent(testLogItem.Properties.Keys,logItemViewModel.Properties.Keys,"Properties dictionary keys are not correctly mapped");
                CollectionAssert.AreEquivalent(testLogItem.Properties.Values,logItemViewModel.Properties.Values,"Properties dictionary values are not correctly mapped");
            }
        }
    }
}