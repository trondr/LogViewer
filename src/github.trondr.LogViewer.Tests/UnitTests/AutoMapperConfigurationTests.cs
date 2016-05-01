using System;
using System.Collections.Generic;
using Common.Logging.Simple;
using github.trondr.LogViewer.Infrastructure;
using github.trondr.LogViewer.Library.Infrastructure;
using github.trondr.LogViewer.Library.Module.Model;
using github.trondr.LogViewer.Library.Module.ViewModels;
using NUnit.Framework;
using LogLevel = Common.Logging.LogLevel;

namespace github.trondr.LogViewer.Tests.UnitTests
{
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