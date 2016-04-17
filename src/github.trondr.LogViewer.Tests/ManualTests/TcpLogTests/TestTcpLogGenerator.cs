using System.Threading;
using log4net;
using log4net.Appender;
using log4net.Core;
using log4net.Layout;
using log4net.Repository.Hierarchy;
using syslog4net.Appender;

namespace github.trondr.LogViewer.Tests.ManualTests.TcpLogTests
{
    public class TestTcpLogGenerator
    {
        private readonly string _remoteAddress;
        private readonly int _remotePort;
        private bool _stop;
        // ReSharper disable once UnusedMember.Local
        private TestTcpLogGenerator()
        {
        }

        public TestTcpLogGenerator(string remoteAddress, int remotePort)
        {
            _remoteAddress = remoteAddress;
            _remotePort = remotePort;
        }

        public void Start()
        {
            _stop = false;
            var logger = ConfigureLogger();
            int i = 0;
            while(!_stop)
            {                
                i++;
                logger.Info("TCP test message " + i);
                Thread.Sleep(500);
            }
        }

        private ILog ConfigureLogger()
        {            
            var loggerRepository = LogManager.GetRepository();  
            loggerRepository.Threshold = Level.Debug;          
            var loggerHierarchy = (Hierarchy)loggerRepository;
            var loggerRoot = loggerHierarchy.Root;
            loggerRoot.AddAppender(GetTcpAppender());
            loggerRepository.Configured = true;
            var logger = LogManager.GetLogger(loggerRepository.Name, "tcplogger");                        
            return logger;
        }

        private IAppender GetTcpAppender()
        {
            var appender = new TcpAppender();
            var ipAddresses = System.Net.Dns.GetHostAddresses(_remoteAddress);
            appender.Name = "tcpLoggerAppender";
            appender.RemoteAddress = ipAddresses[1];
            appender.RemotePort = _remotePort;  
            appender.Layout = new XmlLayoutSchemaLog4j(true);
            appender.Threshold = Level.Debug;
            appender.ActivateOptions();
            return appender;
        }

        public void Stop()
        {
            _stop = true;
        }
    }
}