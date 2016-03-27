using Common.Logging;
using github.trondr.LogViewer.Library.Commands.OpenLog;
using NCmdLiner.Attributes;
using github.trondr.LogViewer.Library.Infrastructure;

namespace github.trondr.LogViewer.Commands
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
