using System.Threading;
using log4net;
using log4net.Appender;
using log4net.Layout;
using log4net.Repository.Hierarchy;
using syslog4net.Appender;

namespace github.trondr.LogViewer.Tests.IntegrationTests
{
    public class TestTcpLogGenerator
    {
        private readonly string _remoteAddress;
        private readonly int _remotePort;
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
            var logger = ConfigureLogger();
            for (int i = 0; i < 1000; i++)
            {
                logger.Info("TCP test message " + i);
                Thread.Sleep(500);
            }
        }

        private ILog ConfigureLogger()
        {
            var loggerRepository = LogManager.GetRepository();            
            var loggerHierarchy = (Hierarchy)loggerRepository;
            var loggerRoot = loggerHierarchy.Root;
            loggerRoot.AddAppender(GetUdpAppender());
            var logger = LogManager.GetLogger(loggerRepository.Name, "tcplogger");            
            return logger;
        }

        private IAppender GetUdpAppender()
        {
            var appender = new TcpAppender();
            var ipAddresses = System.Net.Dns.GetHostAddresses(_remoteAddress);
            appender.RemoteAddress = ipAddresses[1];
            appender.RemotePort = _remotePort;            
            appender.Layout = new XmlLayoutSchemaLog4j(true);
            return appender;
        }
    }
}