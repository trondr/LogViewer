using Common.Logging;
using LogViewer.Library.Infrastructure;
using LogViewer.Library.Module.Commands.OpenLog;
using NCmdLiner.Attributes;

namespace LogViewer.Module.Commands
{
    public class OpenLogsCommandDefinition: CommandDefinition
    {        
        private readonly IOpenLogsCommandProvider _openLogsCommandProvider;
        private readonly ILog _logger;

        public OpenLogsCommandDefinition(IOpenLogsCommandProvider openLogsCommandProvider, ILog logger)
        {            
            _openLogsCommandProvider = openLogsCommandProvider;
            _logger = logger;
        }

        [Command(Description = "Open log file")]
        public int OpenLogs(
            [RequiredCommandParameter(Description = "Log receiver connection strings.", AlternativeName = "cs", ExampleValue = new[]{@"file://c:\temp\productname-username.log",@"file://c:\temp\productname-username.log"} )]
            string[] connectionStrings  
            )
        {                        
            return _openLogsCommandProvider.OpenLogs(connectionStrings);
        }
    }
}
