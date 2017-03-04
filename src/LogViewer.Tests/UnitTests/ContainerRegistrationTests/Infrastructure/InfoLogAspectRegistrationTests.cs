using NUnit.Framework;
using LogViewer.Infrastructure.ContainerExtensions;
using LogViewer.Tests.Common;

namespace LogViewer.Tests.UnitTests.ContainerRegistrationTests.Infrastructure
{
    [TestFixture(Category=TestCategory.UnitTests)]
    public class InfoLogAspectRegistrationTests
    {
        [Test, RequiresSTA]
        public void InfoLogAspectRegistrationTest()
        {
            BootStrapperTestsHelper.CheckThatNumberOfResolvedServicesAre<InfoLogAspect>(1);
            BootStrapperTestsHelper.CheckThatResolvedServiceIsOfInstanceType<InfoLogAspect, InfoLogAspect>();
            BootStrapperTestsHelper.CheckThatResolvedServiceHasSingletonLifeCycle<InfoLogAspect>();
        }
    }
}