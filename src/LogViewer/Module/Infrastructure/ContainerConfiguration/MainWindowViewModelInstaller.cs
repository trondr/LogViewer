using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using LogViewer.Infrastructure.ContainerExtensions;
using LogViewer.Library.Module.ViewModels;

namespace LogViewer.Module.Infrastructure.ContainerConfiguration
{
    [InstallerPriority(InstallerPriorityAttribute.DefaultPriority)]
    public class MainWindowViewModelInstaller: IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {            
            container.Register(Component.For<MainWindowViewModel>().Activator<StrictComponentActivator>());
        }
    }
}