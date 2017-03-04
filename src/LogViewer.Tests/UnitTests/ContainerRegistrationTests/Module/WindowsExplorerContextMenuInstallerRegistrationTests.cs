using NUnit.Framework;
using LogViewer.Library.Module.Common.Install;
using LogViewer.Tests.Common;

namespace LogViewer.Tests.UnitTests.ContainerRegistrationTests.Module
{
    [TestFixture(Category=TestCategory.UnitTests)]
    public class WindowsExplorerContextMenuInstallerRegistrationTests
    {        
        [Test, RequiresSTA]
        public static void WindowsExplorerContextMenuInstallerRegistrationTest()
        {
            BootStrapperTestsHelper.CheckThatNumberOfResolvedServicesAre<IWindowsExplorerContextMenuInstaller>(1);
            BootStrapperTestsHelper.CheckThatResolvedServiceIsOfInstanceType<IWindowsExplorerContextMenuInstaller, WindowsExplorerContextMenuInstaller>();
            BootStrapperTestsHelper.CheckThatResolvedServiceHasSingletonLifeCycle<IWindowsExplorerContextMenuInstaller>();
        }

    }
}