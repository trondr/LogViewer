using System;
using System.Collections.Generic;
using Castle.Facilities.TypedFactory;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Common.Logging;
using Common.Logging.Simple;
using github.trondr.LogViewer.Infrastructure;
using github.trondr.LogViewer.Library.Infrastructure;
using github.trondr.LogViewer.Library.Module.Model;
using NUnit.Framework;
using LogLevel = Common.Logging.LogLevel;

namespace github.trondr.LogViewer.Tests.UnitTests
{
    [TestFixture(Category = "UnitTests")]
    public class LogItemFactoryTests
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
        public void LogItemFactoryTest1()
        {

            using (var testBooStrapper = new TestBootStrapper(GetType()))
            {
                var target = testBooStrapper.Container.Resolve<ILogItemFactory>();

                var expected = new LogItem()
                {
                    Time = DateTime.Now,
                    LogLevel = Library.Module.Model.LogLevel.Error,
                    Logger = "SomeRoot.SomeNameSpace",
                    ThreadId = "1",
                    Message = "Some message",
                    ExceptionString = "Some exception string",
                    Properties = new Dictionary<string, string> { { "username", "ola" }, { "lastname", "Normann" } }
                };

                var actual = target.GetLogItem(expected.Time, expected.LogLevel, expected.Logger, expected.ThreadId, expected.Message, expected.ExceptionString, expected.Properties);
                Assert.IsInstanceOf<LogItem>(actual);                
                Assert.IsNotNull(actual);
                Assert.AreEqual(expected.Time, actual.Time, "Time was not expected.");
                Assert.AreEqual(expected.LogLevel, actual.LogLevel, "LogLevel was not expected.");
                Assert.AreEqual(expected.Logger, actual.Logger, "Logger was not expected.");
                Assert.AreEqual(expected.ThreadId, actual.ThreadId, "ThreadId was not expected.");
                Assert.AreEqual(expected.Message, actual.Message, "Message was not expected.");
                Assert.AreEqual(expected.ExceptionString, actual.ExceptionString, "ExceptionString was not expected.");
                ToDo.Implement(ToDoPriority.Critical, "trondr","Initialization of Dictionary is not working correctly.");
                Assert.AreEqual(expected.Properties.Count, actual.Properties.Count, "Properties.Count was not expected.");
            }
        }

        internal class TestBootStrapper : IDisposable
        {
            readonly ILog _logger;
            private IWindsorContainer _container;

            public TestBootStrapper(Type type)
            {
                _logger = new ConsoleOutLogger(type.Name, LogLevel.Info, true, false, false, "yyyy-MM-dd HH:mm:ss");
            }

            public IWindsorContainer Container
            {
                get
                {
                    if (_container == null)
                    {
                        _container = new WindsorContainer();
                        _container.Register(Component.For<IWindsorContainer>().Instance(_container));

                        //Configure logging
                        _container.Register(Component.For<ILog>().Instance(_logger));

                        //Manual override registrations for interfaces that the interface under test is dependent on
                        //_container.Register(Component.For<ISomeInterface>().Instance(MockRepository.GenerateStub<ISomeInterface>()));

                        //Factory registrations example:
                        _container.AddFacility<TypedFactoryFacility>();
                        _container.Register(Component.For<ITypedFactoryComponentSelector>().ImplementedBy<CustomTypeFactoryComponentSelector>());

                        _container.Register(Component.For<ILogItemFactory>().AsFactory());
                        _container.Register(
                            Component.For<LogItem>()
                                .ImplementedBy<LogItem>()
                                .Named("LogItem")
                                .LifeStyle.Transient);

                        ///////////////////////////////////////////////////////////////////
                        //Automatic registrations
                        ///////////////////////////////////////////////////////////////////
                        //
                        //   Register all command providers and attach logging interceptor
                        //
                        const string libraryRootNameSpace = "github.trondr.LogViewer.Library";

                        //
                        //   Register all singletons found in the library
                        //
                        _container.Register(Classes.FromAssemblyContaining<CommandDefinition>()
                            .InNamespace(libraryRootNameSpace, true)
                            .If(type => Attribute.IsDefined(type, typeof(SingletonAttribute)))
                            .WithService.DefaultInterfaces().LifestyleSingleton());

                        //
                        //   Register all transients found in the library
                        //
                        _container.Register(Classes.FromAssemblyContaining<CommandDefinition>()
                            .InNamespace(libraryRootNameSpace, true)
                            .WithService.DefaultInterfaces().LifestyleTransient());

                    }
                    return _container;
                }

            }

            ~TestBootStrapper()
            {
                Dispose(false);
            }

            public void Dispose()
            {
                Dispose(true);
            }

            protected virtual void Dispose(bool disposing)
            {
                if (disposing)
                {
                    if (_container != null)
                    {
                        _container.Dispose();
                        _container = null;
                    }
                }
            }
        }
    }
}