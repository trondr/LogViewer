using NUnit.Framework;
using LogViewer.Infrastructure.ContainerExtensions;
using LogViewer.Tests.Common;

namespace LogViewer.Tests.UnitTests.ContainerRegistrationTests.Infrastructure
{
    [TestFixture(Category=TestCategory.UnitTests)]
    public class LogFactoryRegistrationTests
    {
        [Test, RequiresSTA]
        public void LogFactoryRegistrationTest()
        {
            BootStrapperTestsHelper.CheckThatNumberOfResolvedServicesAre<ILogFactory>(1);
            BootStrapperTestsHelper.CheckThatResolvedServiceIsOfInstanceType<ILogFactory, LogFactory>();
            BootStrapperTestsHelper.CheckThatResolvedServiceHasSingletonLifeCycle<ILogFactory>();
        }
    }
}