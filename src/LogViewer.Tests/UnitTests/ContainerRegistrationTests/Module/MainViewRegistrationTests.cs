using NUnit.Framework;
using LogViewer.Infrastructure;
using LogViewer.Library.Module.Views;
using LogViewer.Tests.Common;

namespace LogViewer.Tests.UnitTests.ContainerRegistrationTests.Module
{
    [TestFixture(Category=TestCategory.UnitTests)]
    public class MainViewRegistrationTests
    {
        [Test, RequiresSTA]
        public void MainViewRegistrationTest()
        {
            BootStrapperTestsHelper.CheckThatNumberOfResolvedServicesAre<MainView>(1);
            BootStrapperTestsHelper.CheckThatResolvedServiceIsOfInstanceType<MainView, MainView>();
            BootStrapperTestsHelper.CheckThatResolvedServiceHasSingletonLifeCycle<MainView>();            
        }
    }
}