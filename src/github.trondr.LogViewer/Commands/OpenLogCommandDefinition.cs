using Common.Logging;
using github.trondr.LogViewer.Library.Commands.OpenLog;
using NCmdLiner.Attributes;
using github.trondr.LogViewer.Library.Infrastructure;

namespace github.trondr.LogViewer.Commands
{
    public class OpenLogCommandDefinition: CommandDefinition
    {
        private readonly IOpenLogCommandProviderFactory _openLogCommandProviderFactory;
        private readonly ILog _logger;

        public OpenLogCommandDefinition(IOpenLogCommandProviderFactory openLogCommandProviderFactory, ILog logger)
        {
            _openLogCommandProviderFactory = openLogCommandProviderFactory;
            _logger = logger;
        }

        [Command(Description = "Open log file")]
        public int OpenLog(
            [RequiredCommandParameter(Description = "Path to log file.", AlternativeName = "xp", ExampleValue = @"c:\temp")]
            string logFile
            )
        {            
            return _openLogCommandProviderFactory.GetOpenLogCommandProvider().OpenLog(logFile);
        }
    }
}
