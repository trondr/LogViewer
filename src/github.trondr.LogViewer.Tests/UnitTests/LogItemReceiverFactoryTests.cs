using System;
using System.Linq;
using Castle.Core;
using Castle.Facilities.TypedFactory;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Castle.Windsor;
using Common.Logging;
using Common.Logging.Simple;
using github.trondr.LogViewer.Infrastructure;
using github.trondr.LogViewer.Library.Commands.OpenLog;
using github.trondr.LogViewer.Library.Infrastructure;
using github.trondr.LogViewer.Library.Services;
using github.trondr.LogViewer.Library.Services.FileLogItem;
using NUnit.Framework;
using SingletonAttribute = github.trondr.LogViewer.Library.Infrastructure.SingletonAttribute;

namespace github.trondr.LogViewer.Tests.UnitTests
{
    [TestFixture(Category = "UnitTests")]
    public class LogItemReceiverFactoryTests
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
        public void FileLogItemReceiverTest()
        {            
            using(var testBooStrapper = new TestBootStrapper(GetType()))
            {
                var target = testBooStrapper.Container.Resolve<ILogItemHandlerFactory>();
                var fileLogItemConnection = new FileLogItemConnection(null);
                var actual = target.GetLogItemHandlers(fileLogItemConnection).ToList();
                Assert.AreEqual(1, actual.Count);
                Assert.AreEqual(actual[0].GetType(), typeof(FileLogItemHandler));
            }
        }

        [Test]
        public void EventLogItemReceiverTest()
        {            
            using(var testBooStrapper = new TestBootStrapper(GetType()))
            {
                var target = testBooStrapper.Container.Resolve<ILogItemHandlerFactory>();                
                var logItemConnection = new EventLogItemConnection();
                var actual = target.GetLogItemHandlers(logItemConnection).ToList();
                Assert.AreEqual(1, actual.Count);
                Assert.AreEqual(actual[0].GetType(), typeof(EventLogItemHandler));
            }
        }
        
        internal class TestBootStrapper: IDisposable
        {
            readonly ILog _logger;
            private IWindsorContainer _container;

            public TestBootStrapper(Type type)
            {
                _logger = new ConsoleOutLogger(type.Name,LogLevel.Info, true, false,false,"yyyy-MM-dd HH:mm:ss");
            }

            public IWindsorContainer Container
            {
                get
                {
                    if(_container == null)
                    {
                        _container = new WindsorContainer();
                        _container.Register(Component.For<IWindsorContainer>().Instance(_container));
                        _container.AddFacility<TypedFactoryFacility>();

                        _container.Register(Component.For<ITypedFactoryComponentSelector>().ImplementedBy<CustomTypeFactoryComponentSelector>());

                        _container.Register(Component.For<ILogItemHandlerFactory>().AsFactory(new LogItemHandlerSelector()));
                        _container.Register(
                            Classes.FromAssemblyContaining<ILogItemHandlerFactory>()
                            .BasedOn(typeof(ILogItemHandler<>))
                            .WithService
                            .Base()
                            .Configure(registration => registration.LifeStyle.Is(LifestyleType.Transient))
                            );

                        _container.Register(Component.For<IFileLogItemConnectionFactory>().AsFactory());
                        _container.Register(
                            Component.For<IFileLogItemConnection>()
                            .ImplementedBy<FileLogItemConnection>()
                            .Named("FileConnectionString")
                            .LifeStyle.Transient);

                        //Configure logging
                        _container.Register(Component.For<ILog>().Instance(_logger));

                        //Manual override registrations for interfaces that the interface under test is dependent on
                        //_container.Register(Component.For<ISomeInterface>().Instance(MockRepository.GenerateStub<ISomeInterface>()));

                        _container.Register(Classes.FromAssemblyInThisApplication().IncludeNonPublicTypes().BasedOn<ILogItemConnectionStringParser>().WithService.FromInterface());                        
                        
                        _container.Kernel.Resolver.AddSubResolver(new CollectionResolver(_container.Kernel));

                        //Factory registrations example:

                        //_container.Register(Component.For<IFileLogItemReceiverFactory>().AsFactory());
                        //_container.Register(
                        //Component.For<IFileLogItemReceiver>()
                        //    .ImplementedBy<FileLogItemReceiver>()
                        //    .Named("FileLogItemReceiver")
                        //    .LifeStyle.Transient);

                        //_container.Register(Component.For<IFileConnectionStringFactory>().AsFactory());
                        //_container.Register(
                        //    Component.For<IFileConnectionString>()
                        //        .ImplementedBy<FileConnectionString>()
                        //        .Named("FileConnectionString")
                        //        .LifeStyle.Transient);


                        //Factory registrations example:

                        //container.Register(Component.For<ITeamProviderFactory>().AsFactory());
                        //container.Register(
                        //    Component.For<ITeamProvider>()
                        //        .ImplementedBy<CsvTeamProvider>()
                        //        .Named("CsvTeamProvider")
                        //        .LifeStyle.Transient);
                        //container.Register(
                        //    Component.For<ITeamProvider>()
                        //        .ImplementedBy<SqlTeamProvider>()
                        //        .Named("SqlTeamProvider")
                        //        .LifeStyle.Transient);

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
                if(disposing)
                {
                    if(_container != null)
                    {
                        _container.Dispose();
                        _container = null;
                    }
                }
            }
        }
    }
}