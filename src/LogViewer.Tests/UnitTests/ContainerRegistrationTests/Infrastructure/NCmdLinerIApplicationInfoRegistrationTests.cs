using NCmdLiner;
using NUnit.Framework;
using LogViewer.Tests.Common;

namespace LogViewer.Tests.UnitTests.ContainerRegistrationTests.Infrastructure
{
    [TestFixture(Category=TestCategory.UnitTests)]
    public class NCmdLinerIApplicationInfoRegistrationTests
    {
        [Test, RequiresSTA]
        public void NCmdLinerIApplicationInfoRegistrationTest()
        {
            BootStrapperTestsHelper.CheckThatNumberOfResolvedServicesAre<IApplicationInfo>(1);
            BootStrapperTestsHelper.CheckThatResolvedServiceIsOfInstanceType<IApplicationInfo, ApplicationInfo>();
            BootStrapperTestsHelper.CheckThatResolvedServiceHasSingletonLifeCycle<IApplicationInfo>();
        }
    }
}