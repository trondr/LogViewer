using Castle.Facilities.TypedFactory;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using LogViewer.Infrastructure.ContainerExtensions;
using LogViewer.Library.Module.Services.RemotingLog;

namespace LogViewer.Module.Infrastructure.ContainerConfiguration
{
    [InstallerPriority(InstallerPriorityAttribute.DefaultPriority)]
    public class RemotingLogItemConnectionFactoryContainerInstaller: IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<IRemotingLogItemConnectionFactory>().AsFactory());
            container.Register(
                Component.For<IRemotingLogItemConnection>()
                    .ImplementedBy<RemotingLogItemConnection>()
                    .Named(nameof(RemotingLogItemConnection))
                    .LifeStyle.Transient);            
        }
    }
}