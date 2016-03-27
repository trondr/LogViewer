using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Castle.Core;
using Castle.Core.Internal;
using Castle.DynamicProxy.Internal;
using Castle.Facilities.Logging;
using Castle.Facilities.TypedFactory;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Common.Logging;
using github.trondr.LogViewer.Library.Commands.OpenLog;
using NCmdLiner;
using github.trondr.LogViewer.Library.Infrastructure;
using github.trondr.LogViewer.Library.Services;
using github.trondr.LogViewer.Library.Services.FileLogItem;
using github.trondr.LogViewer.Library.Services.RandomLogItem;
using github.trondr.LogViewer.Library.ViewModels;
using github.trondr.LogViewer.Library.Views;
using SingletonAttribute = github.trondr.LogViewer.Library.Infrastructure.SingletonAttribute;

namespace github.trondr.LogViewer.Infrastructure
{
    public class ContainerInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<IWindsorContainer>().Instance(container));
            container.AddFacility<TypedFactoryFacility>();
            container.Register(Component.For<ITypedFactoryComponentSelector>().ImplementedBy<CustomTypeFactoryComponentSelector>());
            
            //Configure logging
            ILoggingConfiguration loggingConfiguration = new LoggingConfiguration();
            log4net.GlobalContext.Properties["LogFile"] = Path.Combine(loggingConfiguration.LogDirectoryPath, loggingConfiguration.LogFileName);
            log4net.Config.XmlConfigurator.ConfigureAndWatch(new FileInfo(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile));
            
            var applicationRootNameSpace = typeof (Program).Namespace;

            container.AddFacility<LoggingFacility>(f => f.UseLog4Net().ConfiguredExternally());
            container.Kernel.Register(Component.For<ILog>().Instance(LogManager.GetLogger(applicationRootNameSpace))); //Default logger
            var logFactory = new LogFactory();
            container.Register(Component.For<ILogFactory>().Instance(logFactory).LifestyleSingleton());
            container.Kernel.Resolver.AddSubResolver(new LoggerSubDependencyResolver(logFactory)); //Enable injection of class specific loggers
            
            //Manual registrations
            container.Register(Component.For<MainWindow>().Activator<StrictComponentActivator>());
            container.Register(Component.For<MainView>().Activator<StrictComponentActivator>());
            container.Register(Component.For<MainViewModel>().Activator<StrictComponentActivator>());

            container.Register(Classes.FromAssemblyInThisApplication().IncludeNonPublicTypes().BasedOn<ILogItemConnectionStringParser>().WithServiceAllInterfaces());
            container.Kernel.Resolver.AddSubResolver(new CollectionResolver(container.Kernel));

            //Factory registrations:
            container.Register(Component.For<ILogItemHandlerFactory>().AsFactory(new LogItemHandlerSelector()));
            container.Register(
                Classes.FromAssemblyContaining<ILogItemHandlerFactory>()
                .BasedOn(typeof(ILogItemHandler<>))
                .WithService
                .Base()
                .Configure(registration => registration.LifeStyle.Is(LifestyleType.Transient))
                );

            container.Register(Component.For<IFileLogItemConnectionFactory>().AsFactory());
            container.Register(
                Component.For<IFileLogItemConnection>()
                    .ImplementedBy<FileLogItemConnection>()
                    .Named("FileLogItemConnection")
                    .LifeStyle.Transient);

            container.Register(Component.For<IRandomLogItemConnectionFactory>().AsFactory());
            container.Register(
                Component.For<IRandomLogItemConnection>()
                    .ImplementedBy<RandomLogItemConnection>()
                    .Named("RandomLogItemConnection")
                    .LifeStyle.Transient);

            container.Register(Component.For<IInvocationLogStringBuilder>().ImplementedBy<InvocationLogStringBuilder>().LifestyleSingleton());
            
            ///////////////////////////////////////////////////////////////////
            //Automatic registrations
            ///////////////////////////////////////////////////////////////////
            //
            //   Register all interceptors
            //
            container.Register(Classes.FromAssemblyInThisApplication()
                .Pick().If(type => type.Name.EndsWith("Aspect")).LifestyleSingleton());
            //
            //   Register all command providers and attach logging interceptor
            //
            
            container.Register(Classes.FromAssemblyContaining<CommandProvider>()
                .InNamespace(_libraryRootNameSpace, true)
                .If(type => type.Is<CommandProvider>())
                .Configure(registration => registration.Interceptors(new[] { typeof(InfoLogAspect) }))
                .WithService.DefaultInterfaces().LifestyleTransient()                
            );
            //
            //   Register all command definitions
            //
            container.Register(
                Classes.FromAssemblyInThisApplication()
                .BasedOn<CommandDefinition>()
                .WithServiceBase()
                );
            //
            //   Register all singletons found in the library
            //
            container.Register(Classes.FromAssemblyContaining<CommandDefinition>()
                .InNamespace(_libraryRootNameSpace, true)
                .If(type => Attribute.IsDefined(type, typeof(SingletonAttribute)))
                .WithService.DefaultInterfaces().LifestyleSingleton());
            //
            //   Register all transients found in the library
            //
            container.Register(Classes.FromAssemblyContaining<CommandDefinition>()
                .InNamespace(_libraryRootNameSpace, true).If(IfFilter)
                .WithServiceDefaultInterfaces()
                .LifestyleTransient());
            
            IApplicationInfo applicationInfo = new ApplicationInfo();
            container.Register(Component.For<IApplicationInfo>().Instance(applicationInfo).LifestyleSingleton());
        }

        private bool IfFilter(Type type)
        {
            var interfaces = type.GetAllInterfaces();
            if(interfaces != null && interfaces.Length > 0)
            {
                var hasMatchingIterfaces = interfaces.Select(type1 => type1.FullName.Contains(_libraryRootNameSpace)).Any();
                if(hasMatchingIterfaces)
                {
                    Console.WriteLine(type.FullName);
                    return true;
                }
            }
            return false;
        }

        const string _libraryRootNameSpace = "github.trondr.LogViewer.Library";        
    }
}
