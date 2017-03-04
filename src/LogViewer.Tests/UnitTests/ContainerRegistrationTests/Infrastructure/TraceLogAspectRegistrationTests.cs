using NUnit.Framework;
using LogViewer.Infrastructure.ContainerExtensions;
using LogViewer.Tests.Common;

namespace LogViewer.Tests.UnitTests.ContainerRegistrationTests.Infrastructure
{
    [TestFixture(Category=TestCategory.UnitTests)]
    public class TraceLogAspectRegistrationTests
    {
        [Test, RequiresSTA]
        public void TraceLogAspectRegistrationTest()
        {
            BootStrapperTestsHelper.CheckThatNumberOfResolvedServicesAre<TraceLogAspect>(1);
            BootStrapperTestsHelper.CheckThatResolvedServiceIsOfInstanceType<TraceLogAspect, TraceLogAspect>();
            BootStrapperTestsHelper.CheckThatResolvedServiceHasSingletonLifeCycle<TraceLogAspect>();
        }
    }
}