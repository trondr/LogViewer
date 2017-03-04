using NUnit.Framework;
using LogViewer.Library.Infrastructure;
using LogViewer.Library.Module.Commands.Example;
using LogViewer.Tests.Common;

namespace LogViewer.Tests.UnitTests.ContainerRegistrationTests.Module
{
    [TestFixture(Category=TestCategory.UnitTests)]
    public class ExampleCommandProviderRegistrationTests
    {        
        [Test, RequiresSTA]
        public static void ExampleCommandProviderRegistrationTest()
        {
            BootStrapperTestsHelper.CheckThatNumberOfResolvedServicesAre<IExampleCommandProvider>(1);
            BootStrapperTestsHelper.CheckThatResolvedServiceIsOfInstanceTypeName<IExampleCommandProvider>("IExampleCommandProviderProxy");
            BootStrapperTestsHelper.CheckThatResolvedServiceHasSingletonLifeCycle<IExampleCommandProvider>();
        }

    }
}