using Castle.Facilities.TypedFactory;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using LogViewer.Infrastructure.ContainerExtensions;
using LogViewer.Library.Module.Services.WinDebugLog;

namespace LogViewer.Module.Infrastructure.ContainerConfiguration
{
    [InstallerPriority(InstallerPriorityAttribute.DefaultPriority)]
    public class WinDebugLogItemConnectionFactoryContainerInstaller: IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<IWinDebugLogItemConnectionFactory>().AsFactory());
            container.Register(
                Component.For<IWinDebugLogItemConnection>()
                    .ImplementedBy<WinDebugLogItemConnection>()
                    .Named(nameof(WinDebugLogItemConnection))
                    .LifeStyle.Transient);            
        }
    }
}