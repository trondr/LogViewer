using NUnit.Framework;
using LogViewer.Library.Infrastructure;
using LogViewer.Tests.Common;

namespace LogViewer.Tests.UnitTests.ContainerRegistrationTests.Infrastructure
{
    [TestFixture(Category=TestCategory.UnitTests)]
    public class TypeMapperRegistrationTests
    {
        [Test, RequiresSTA]
        public void TypeMapperRegistrationTest()
        {
            BootStrapperTestsHelper.CheckThatNumberOfResolvedServicesAre<ITypeMapper>(1);
            BootStrapperTestsHelper.CheckThatResolvedServiceIsOfInstanceType<ITypeMapper, TypeMapper>();
            BootStrapperTestsHelper.CheckThatResolvedServiceHasSingletonLifeCycle<ITypeMapper>();
        }
    }
}