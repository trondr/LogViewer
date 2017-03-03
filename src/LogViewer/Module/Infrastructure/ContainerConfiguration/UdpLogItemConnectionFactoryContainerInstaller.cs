using Castle.Facilities.TypedFactory;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using LogViewer.Infrastructure.ContainerExtensions;
using LogViewer.Library.Module.Services.UdpLog;

namespace LogViewer.Module.Infrastructure.ContainerConfiguration
{
    [InstallerPriority(InstallerPriorityAttribute.DefaultPriority)]
    public class UdpLogItemConnectionFactoryContainerInstaller: IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<IUdpLogItemConnectionFactory>().AsFactory());
            container.Register(
                Component.For<IUdpLogItemConnection>()
                    .ImplementedBy<UdpLogItemConnection>()
                    .Named(nameof(UdpLogItemConnection))
                    .LifeStyle.Transient);            
        }
    }
}