using System.Threading;
using log4net;
using log4net.Appender;
using log4net.Core;
using log4net.Layout;
using log4net.Repository.Hierarchy;

namespace github.trondr.LogViewer.Tests.ManualTests.RemotingLogTest
{
    public class TestRemotingLogGenerator
    {
        private readonly string _sink;        
        private bool _stop;
        // ReSharper disable once UnusedMember.Local
        private TestRemotingLogGenerator()
        {
        }

        public TestRemotingLogGenerator(string sink)
        {
            _sink = sink;            
        }

        public void Start()
        {
            _stop = false;
            var logger = ConfigureLogger();
            int i = 0;
            while(!_stop)
            {                
                i++;
                logger.Info("Remoting test message " + i);
                Thread.Sleep(500);
            }
        }

        private ILog ConfigureLogger()
        {            
            var loggerRepository = LogManager.GetRepository();  
            loggerRepository.Threshold = Level.Debug;          
            var loggerHierarchy = (Hierarchy)loggerRepository;
            var loggerRoot = loggerHierarchy.Root;
            loggerRoot.AddAppender(GetRemotingAppender());
            loggerRepository.Configured = true;
            var logger = LogManager.GetLogger(loggerRepository.Name, "remotinglogger");
            return logger;
        }

        private IAppender GetRemotingAppender()
        {
            var appender = new RemotingAppender();            
            appender.Name = "remotingLoggerAppender";
            appender.Sink = _sink; //Example: tcp://localhost:7070/LoggingSink
            appender.BufferSize = 1;
            appender.Lossy = false;            
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