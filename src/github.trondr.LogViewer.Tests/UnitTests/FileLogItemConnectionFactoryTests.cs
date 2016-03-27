using System;
using Castle.Facilities.TypedFactory;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Common.Logging;
using Common.Logging.Simple;
using github.trondr.LogViewer.Infrastructure;
using github.trondr.LogViewer.Library.Commands.OpenLog;
using github.trondr.LogViewer.Library.Infrastructure;
using github.trondr.LogViewer.Library.Services;
using github.trondr.LogViewer.Library.Services.FileLogItem;
using NUnit.Framework;

namespace github.trondr.LogViewer.Tests.UnitTests
{
    [TestFixture(Category = "UnitTests")]
    public class FileLogItemConnectionFactoryTests
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
        public void FileLogItemConnectionFactoryTest1()
        {
            
            using(var testBooStrapper = new TestBootStrapper(GetType()))
            {
                var target = testBooStrapper.Container.Resolve<IFileLogItemConnectionFactory>();
                string testConnectionString = @"file://c:\some\temp\file.log";
                var actual = target.GetFileLogItemConnection(testConnectionString);
                Assert.IsNotNull(actual);
                Assert.IsTrue(actual.GetType() == typeof(FileLogItemConnection),"Not of type FileLogItemConnection");
                Assert.AreEqual(testConnectionString, actual.Value, "Connection string not equal");
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

                        //Configure logging
                        _container.Register(Component.For<ILog>().Instance(_logger));
                        _container.Register(Component.For<ITypedFactoryComponentSelector>().ImplementedBy<CustomTypeFactoryComponentSelector>());

                        //Manual override registrations for interfaces that the interface under test is dependent on
                        //_container.Register(Component.For<ISomeInterface>().Instance(MockRepository.GenerateStub<ISomeInterface>()));

                        //Factory registrations example:

                        _container.Register(Component.For<IFileLogItemConnectionFactory>().AsFactory());
                        _container.Register(
                            Component.For<IFileLogItemConnection>()
                                .ImplementedBy<FileLogItemConnection>()
                                .Named("FileLogItemConnection")
                                .LifeStyle.Transient);
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