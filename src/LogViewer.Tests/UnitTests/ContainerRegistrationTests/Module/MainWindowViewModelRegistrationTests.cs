using NUnit.Framework;
using LogViewer.Library.Module.ViewModels;
using LogViewer.Tests.Common;

namespace LogViewer.Tests.UnitTests.ContainerRegistrationTests.Module
{
    [TestFixture(Category = TestCategory.UnitTests)]
    public class MainWindowViewModelRegistrationTests
    {
        [Test, RequiresSTA]
        public void MainWindowViewModelRegistrationTest()
        {
            BootStrapperTestsHelper.CheckThatNumberOfResolvedServicesAre<MainWindowViewModel>(1);
            BootStrapperTestsHelper.CheckThatResolvedServiceIsOfInstanceType<MainWindowViewModel, MainWindowViewModel>();
            BootStrapperTestsHelper.CheckThatResolvedServiceHasSingletonLifeCycle<MainWindowViewModel>();
        }
    }
}