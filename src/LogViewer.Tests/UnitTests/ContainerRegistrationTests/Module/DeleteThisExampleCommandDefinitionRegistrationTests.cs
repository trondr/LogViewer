using NUnit.Framework;
using LogViewer.Module.Commands;
using LogViewer.Library.Infrastructure;
using LogViewer.Tests.Common;

namespace LogViewer.Tests.UnitTests.ContainerRegistrationTests.Module
{
    [TestFixture(Category=TestCategory.UnitTests)]
    public class DeleteThisExampleCommandDefinitionRegistrationTests
    {        
        [Test, RequiresSTA]
        public static void ExampleCommandDefinitionRegistrationTest()
        {
            //BootStrapperTestsHelper.CheckThatOneOfTheResolvedServicesAre<CommandDefinition, ExampleCommandDefinition>("Not registered: " + typeof(ExampleCommandDefinition).FullName);
        }        
    }
}