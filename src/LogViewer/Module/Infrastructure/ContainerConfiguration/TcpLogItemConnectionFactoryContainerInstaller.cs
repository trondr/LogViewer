using Castle.Facilities.TypedFactory;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using LogViewer.Infrastructure.ContainerExtensions;
using LogViewer.Library.Module.Services.TcpLog;

namespace LogViewer.Module.Infrastructure.ContainerConfiguration
{
    [InstallerPriority(InstallerPriorityAttribute.DefaultPriority)]
    public class TcpLogItemConnectionFactoryContainerInstaller: IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<ITcpLogItemConnectionFactory>().AsFactory());
            container.Register(
                Component.For<ITcpLogItemConnection>()
                    .ImplementedBy<TcpLogItemConnection>()
                    .Named(nameof(TcpLogItemConnection))
                    .LifeStyle.Transient);            
        }
    }
}