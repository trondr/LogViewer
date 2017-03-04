using Castle.Facilities.TypedFactory;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using LogViewer.Infrastructure.ContainerExtensions;
using LogViewer.Library.Module.Services.FileLog;

namespace LogViewer.Module.Infrastructure.ContainerConfiguration
{
    [InstallerPriority(InstallerPriorityAttribute.DefaultPriority)]
    public class FileLogItemConnectionFactoryContainerInstaller: IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
             container.Register(Component.For<IFileLogItemConnectionFactory>().AsFactory());
            container.Register(
                Component.For<IFileLogItemConnection>()
                    .ImplementedBy<FileLogItemConnection>()
                    .Named(nameof(FileLogItemConnection))
                    .LifeStyle.Transient);            
        }
    }
}
