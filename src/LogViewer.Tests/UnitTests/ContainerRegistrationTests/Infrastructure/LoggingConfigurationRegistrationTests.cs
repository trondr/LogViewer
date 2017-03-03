using NUnit.Framework;
using LogViewer.Library.Infrastructure;
using LogViewer.Tests.Common;

namespace LogViewer.Tests.UnitTests.ContainerRegistrationTests.Infrastructure
{
    [TestFixture(Category=TestCategory.UnitTests)]
    public class LoggingConfigurationRegistrationTests
    {
        [Test, RequiresSTA]
        public void LoggingConfigurationRegistrationTest()
        {
            BootStrapperTestsHelper.CheckThatNumberOfResolvedServicesAre<ILoggingConfiguration>(1);
            BootStrapperTestsHelper.CheckThatResolvedServiceIsOfInstanceType<ILoggingConfiguration, LoggingConfiguration>();
            BootStrapperTestsHelper.CheckThatResolvedServiceHasSingletonLifeCycle<ILoggingConfiguration>();
        }
    }
}