using System;
using System.IO;
using Castle.Core;
using Castle.Core.Internal;
using Castle.Facilities.TypedFactory;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Common.Logging;
using NCmdLiner;
using github.trondr.LogViewer.Library.Infrastructure;
using github.trondr.LogViewer.Library.Module.Model;
using github.trondr.LogViewer.Library.Module.Services;
using github.trondr.LogViewer.Library.Module.Services.EventLogItem;
using github.trondr.LogViewer.Library.Module.Services.FileLogItem;
using github.trondr.LogViewer.Library.Module.Services.RandomLogItem;
using github.trondr.LogViewer.Library.Module.ViewModels;
using github.trondr.LogViewer.Library.Module.Views;
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
            container.Register(Component.For<IMessenger>().ImplementedBy<NotepadMessenger>());

            //Configure logging
            ILoggingConfiguration loggingConfiguration = new LoggingConfiguration();
            log4net.GlobalContext.Properties["LogFile"] = Path.Combine(loggingConfiguration.LogDirectoryPath, loggingConfiguration.LogFileName);
            log4net.Config.XmlConfigurator.ConfigureAndWatch(new FileInfo(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile));
            var applicationRootNameSpace = typeof(Program).Namespace;
            container.Kernel.Register(Component.For<ILog>().Instance(LogManager.GetLogger(applicationRootNameSpace))); //Default logger
            container.Kernel.Resolver.AddSubResolver(new LoggerSubDependencyResolver()); //Enable injection of class specific loggers

            //Manual registrations
            container.Register(Component.For<MainWindow>().Activator<StrictComponentActivator>());
            container.Register(Component.For<MainView>().Activator<StrictComponentActivator>());
            container.Register(Component.For<MainViewModel>().Activator<StrictComponentActivator>());

            container.Register(Classes.FromAssemblyInThisApplication().IncludeNonPublicTypes().BasedOn<ILogItemConnectionStringParser>().WithServiceAllInterfaces());
            container.Kernel.Resolver.AddSubResolver(new CollectionResolver(container.Kernel));

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

            container.Register(Component.For<IEventLogItemConnectionFactory>().AsFactory());
            container.Register(
                Component.For<IEventLogItemConnection>()
                    .ImplementedBy<EventLogItemConnection>()
                    .Named("EventLogItemConnection")
                    .LifeStyle.Transient);

            container.Register(Component.For<IRandomLogItemConnectionFactory>().AsFactory());
            container.Register(
                Component.For<IRandomLogItemConnection>()
                    .ImplementedBy<RandomLogItemConnection>()
                    .Named("RandomLogItemConnection")
                    .LifeStyle.Transient);

            container.Register(Component.For<ILogItemFactory>().AsFactory());
            container.Register(
                Component.For<LogItem>()
                    .ImplementedBy<LogItem>()
                    .Named("LogItem")
                    .LifeStyle.Transient);

            container.Register(Component.For<IInvocationLogStringBuilder>().ImplementedBy<InvocationLogStringBuilder>().LifestyleSingleton());
            container.Register(Component.For<ILogFactory>().ImplementedBy<LogFactory>().LifestyleSingleton());

            container.Kernel.Resolver.AddSubResolver(new CollectionResolver(container.Kernel));
            container.Register(Classes.FromAssemblyContaining<ITypeMapper>().IncludeNonPublicTypes().BasedOn<AutoMapper.Profile>().WithService.Base());
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
            const string libraryRootNameSpace = "github.trondr.LogViewer.Library";
            container.Register(Classes.FromAssemblyContaining<CommandProvider>()
                .InNamespace(libraryRootNameSpace, true)
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
                .InNamespace(libraryRootNameSpace, true)
                .If(type => Attribute.IsDefined(type, typeof(SingletonAttribute)))
                .WithService.FirstInterface().LifestyleSingleton());
            //
            //   Register all transients found in the library
            //
            container.Register(Classes.FromAssemblyContaining<CommandDefinition>()
                .InNamespace(libraryRootNameSpace, true)
                .WithService.FirstInterface().LifestyleTransient());

            IApplicationInfo applicationInfo = new ApplicationInfo();
            container.Register(Component.For<IApplicationInfo>().Instance(applicationInfo).LifestyleSingleton());
        }
    }
}
