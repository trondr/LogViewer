using NUnit.Framework;
using LogViewer.Infrastructure;
using LogViewer.Library.Module.Views;
using LogViewer.Tests.Common;

namespace LogViewer.Tests.UnitTests.ContainerRegistrationTests.Module
{
    [TestFixture(Category=TestCategory.UnitTests)]
    public class MainWindowRegistrationTests
    {
        [Test, RequiresSTA]
        public void MainWindowRegistrationTest()
        {
            BootStrapperTestsHelper.CheckThatNumberOfResolvedServicesAre<MainWindow>(1);
            BootStrapperTestsHelper.CheckThatResolvedServiceIsOfInstanceType<MainWindow, MainWindow>();
            BootStrapperTestsHelper.CheckThatResolvedServiceHasSingletonLifeCycle<MainWindow>();
            using (var bootStrapper = new BootStrapper())
            {
                var target = bootStrapper.Container.ResolveAll<MainWindow>();
                Assert.IsNotNull(target[0].ViewModel, "View was null");                
            }
        }
    }
}