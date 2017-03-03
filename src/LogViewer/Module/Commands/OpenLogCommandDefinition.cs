using Common.Logging;
using LogViewer.Library.Infrastructure;
using LogViewer.Library.Module.Commands.OpenLog;
using NCmdLiner.Attributes;

namespace LogViewer.Module.Commands
{
    public class OpenLogCommandDefinition: CommandDefinition
    {        
        private readonly IOpenLogCommandProvider _openLogCommandProvider;
        private readonly ILog _logger;

        public OpenLogCommandDefinition(IOpenLogCommandProvider openLogCommandProvider, ILog logger)
        {            
            _openLogCommandProvider = openLogCommandProvider;
            _logger = logger;
        }

        [Command(Description = "Open log file")]
        public int OpenLogs(
            [RequiredCommandParameter(Description = "Log receiver connection strings.", AlternativeName = "cs", ExampleValue = new[]{@"file://c:\temp\productname-username.log",@"file://c:\temp\productname-username.log"} )]
            string[] connectionStrings  
            )
        {                        
            return _openLogCommandProvider.OpenLogs(connectionStrings);
        }
    }
}
