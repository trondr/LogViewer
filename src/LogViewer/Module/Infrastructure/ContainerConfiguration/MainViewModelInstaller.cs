using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using LogViewer.Infrastructure.ContainerExtensions;
using LogViewer.Library.Module.ViewModels;

namespace LogViewer.Module.Infrastructure.ContainerConfiguration
{
    [InstallerPriority(InstallerPriorityAttribute.DefaultPriority)]
    public class MainViewModelInstaller: IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {            
            container.Register(Component.For<MainViewModel>().Activator<StrictComponentActivator>());            
        }
    }
}