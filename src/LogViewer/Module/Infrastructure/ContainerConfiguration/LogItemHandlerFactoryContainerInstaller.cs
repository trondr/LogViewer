using Castle.Core;
using Castle.Facilities.TypedFactory;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using LogViewer.Infrastructure.ContainerExtensions;
using LogViewer.Library.Module.Services;
using LogViewer.Module.Infrastructure.ContainerExtensions;

namespace LogViewer.Module.Infrastructure.ContainerConfiguration
{
    [InstallerPriority(InstallerPriorityAttribute.DefaultPriority)]
    public class LogItemHandlerFactoryContainerInstaller: IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<ILogItemHandlerFactory>().AsFactory(new LogItemHandlerSelector()));
            container.Register(
                Classes.FromAssemblyContaining<ILogItemHandlerFactory>()
                    .BasedOn(typeof(ILogItemHandler<>))
                    .WithService
                    .Base()
                    .Configure(registration => registration.LifeStyle.Is(LifestyleType.Transient))
            );      
        }
    }
}