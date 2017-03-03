using NUnit.Framework;
using LogViewer.Infrastructure.ContainerExtensions;
using LogViewer.Tests.Common;

namespace LogViewer.Tests.UnitTests.ContainerRegistrationTests.Infrastructure
{
    [TestFixture(Category=TestCategory.UnitTests)]
    public class InvocationLogStringBuilderRegistrationTests
    {
        [Test, RequiresSTA]
        public void InvocationLogStringBuilderRegistrationTest()
        {
            BootStrapperTestsHelper.CheckThatNumberOfResolvedServicesAre<IInvocationLogStringBuilder>(1);
            BootStrapperTestsHelper.CheckThatResolvedServiceIsOfInstanceType<IInvocationLogStringBuilder, InvocationLogStringBuilder>();
        }
    }
}