using Castle.Facilities.TypedFactory;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using LogViewer.Infrastructure.ContainerExtensions;
using LogViewer.Library.Module.Services.EventLog;

namespace LogViewer.Module.Infrastructure.ContainerConfiguration
{
    [InstallerPriority(InstallerPriorityAttribute.DefaultPriority)]
    public class EventLogItemConnectionFactoryContainerInstaller: IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<IEventLogItemConnectionFactory>().AsFactory());
            container.Register(
                Component.For<IEventLogItemConnection>()
                    .ImplementedBy<EventLogItemConnection>()
                    .Named(nameof(EventLogItemConnection))
                    .LifeStyle.Transient);            
        }
    }
}