using Castle.Facilities.TypedFactory;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using LogViewer.Infrastructure.ContainerExtensions;
using LogViewer.Library.Module.Services.RandomLog;

namespace LogViewer.Module.Infrastructure.ContainerConfiguration
{
    [InstallerPriority(InstallerPriorityAttribute.DefaultPriority)]
    public class RandomLogItemConnectionFactoryContainerInstaller: IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<IRandomLogItemConnectionFactory>().AsFactory());
            container.Register(
                Component.For<IRandomLogItemConnection>()
                    .ImplementedBy<RandomLogItemConnection>()
                    .Named(nameof(RandomLogItemConnection))
                    .LifeStyle.Transient);            
        }
    }
}