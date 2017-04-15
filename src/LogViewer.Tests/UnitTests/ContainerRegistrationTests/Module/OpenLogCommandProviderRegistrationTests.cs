using LogViewer.Library.Module.Commands.OpenLog;
using NUnit.Framework;

using LogViewer.Tests.Common;

namespace LogViewer.Tests.UnitTests.ContainerRegistrationTests.Module
{
    [TestFixture(Category=TestCategory.UnitTests)]
    public class OpenLogCommandProviderRegistrationTests
    {        
        [Test, RequiresSTA]
        public static void OpenLogCommandProviderRegistrationTest()
        {
            BootStrapperTestsHelper.CheckThatNumberOfResolvedServicesAre<IOpenLogsCommandProvider>(1);
            BootStrapperTestsHelper.CheckThatResolvedServiceIsOfInstanceTypeName<IOpenLogsCommandProvider>("IOpenLogCommandProviderProxy");
            BootStrapperTestsHelper.CheckThatResolvedServiceHasSingletonLifeCycle<IOpenLogsCommandProvider>();
        }
    }
}