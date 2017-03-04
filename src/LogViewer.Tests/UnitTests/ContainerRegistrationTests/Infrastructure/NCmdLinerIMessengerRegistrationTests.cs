using NCmdLiner;
using NUnit.Framework;
using LogViewer.Infrastructure;
using LogViewer.Tests.Common;

namespace LogViewer.Tests.UnitTests.ContainerRegistrationTests.Infrastructure
{
    [TestFixture(Category=TestCategory.UnitTests)]
    public class NCmdLinerIMessengerRegistrationTests
    {
        [Test, RequiresSTA]
        public void NCmdLinerIMessengerRegistrationTest()
        {
            BootStrapperTestsHelper.CheckThatNumberOfResolvedServicesAre<IMessenger>(1);
            BootStrapperTestsHelper.CheckThatResolvedServiceIsOfInstanceType<IMessenger, NotepadMessenger>();
            BootStrapperTestsHelper.CheckThatResolvedServiceHasSingletonLifeCycle<IMessenger>();
        }
    }
}