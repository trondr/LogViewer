using System;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Common.Logging;
using Common.Logging.Simple;
using github.trondr.LogViewer.Library.Infrastructure;
using github.trondr.LogViewer.Library.ViewModels;
using NUnit.Framework;
using LogLevel = Common.Logging.LogLevel;

namespace github.trondr.LogViewer.Tests.UnitTests
{
        [TestFixture(Category = "UnitTests")]
    public class LoggerViewModelProviderTests
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
        public void LoggerViewModelProviderParentTest()
        {
            
            using(var testBooStrapper = new TestBootStrapper(GetType()))
            {
                var target = testBooStrapper.Container.Resolve<ILoggerViewModelProvider>();
                var actual = target.GetLogger("Company.Product.Class");
                Assert.IsNotNull(actual, "Logger view model was null");
                Assert.AreEqual("Company.Product.Class", actual.Name);
                Assert.AreEqual("Class", actual.DisplayName);
                Assert.IsNotNull(actual.Parent, "Parent logger view model is null");
                Assert.AreEqual("Company.Product", actual.Parent.Name);
                
                Assert.IsNotNull(actual.Parent.Parent, "Parent of parent logger view model is null");
                Assert.IsNotNull(actual.Parent.Parent.Parent, "Parent of parent logger view model is null");
                Assert.AreEqual("root", actual.Parent.Parent.Parent.Name);
                Assert.IsNull(actual.Parent.Parent.Parent.Parent, "Parent of parent of parent logger view model is not null");
            }
        }

        [Test]
        public void LoggerViewModelProviderChildrenCountTest()
        {
            using (var testBooStrapper = new TestBootStrapper(GetType()))
            {
                var target = testBooStrapper.Container.Resolve<ILoggerViewModelProvider>();
                var logger1 = target.GetLogger("Company.Product.Class1");
                var logger2 = target.GetLogger("Company.Product.Class2");
                var logger3 = target.GetLogger("Company.Product.Class3");
                var parent = target.GetLogger("Company.Product");
                Assert.IsNotNull(parent, "Logger view model was null");
                Assert.AreEqual("Company.Product", parent.Name);
                Assert.IsNotNull(parent.Parent, "Parent logger view model is null");
                Assert.AreEqual("Company", parent.Parent.Name);
                Assert.AreEqual(3, parent.Children.Count, "The number of children are not expected");
            }
        }

        [Test]
        public void LoggerViewModelProviderIsVisibleTest()
        {

            using (var testBooStrapper = new TestBootStrapper(GetType()))
            {
                var target = testBooStrapper.Container.Resolve<ILoggerViewModelProvider>();
                var logger1 = target.GetLogger("Company.Product.Class1");
                var logger2 = target.GetLogger("Company.Product.Class2");
                var logger3 = target.GetLogger("Company.Product.Class3");
                var parent = target.GetLogger("Company.Product");
                Assert.IsNotNull(parent, "Logger view model was null");
                Assert.AreEqual("Company.Product", parent.Name);
                Assert.IsNotNull(parent.Parent, "Parent logger view model was null");
                Assert.AreEqual("Company", parent.Parent.Name);
                Assert.AreEqual(3, parent.Children.Count, "The number of children are not expected");
                
                Assert.IsTrue(parent.IsVisible,"Parent is not visible");
                Assert.IsTrue(logger1.IsVisible,"logger1 is not visible");
                Assert.IsTrue(logger2.IsVisible,"logger2 is not visible");
                Assert.IsTrue(logger3.IsVisible,"logger3 is not visible");
                
                parent.IsVisible = false;

                Assert.IsFalse(parent.IsVisible,"Parent is visible");
                Assert.IsFalse(logger1.IsVisible,"logger1 is visible");
                Assert.IsFalse(logger2.IsVisible,"logger2 is visible");
                Assert.IsFalse(logger3.IsVisible,"logger3 is visible");

                parent.IsVisible = true;

                Assert.IsTrue(parent.IsVisible,"Parent is not visible");
                Assert.IsTrue(logger1.IsVisible,"logger1 is not visible");
                Assert.IsTrue(logger2.IsVisible,"logger2 is not visible");
                Assert.IsTrue(logger3.IsVisible,"logger3 is not visible");
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
            
                        //Configure logging
                        _container.Register(Component.For<ILog>().Instance(_logger));
            
                        //Manual override registrations for interfaces that the interface under test is dependent on
                        //_container.Register(Component.For<ISomeInterface>().Instance(MockRepository.GenerateStub<ISomeInterface>()));

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