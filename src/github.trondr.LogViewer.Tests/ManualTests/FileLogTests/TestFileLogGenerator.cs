using System;
using System.Threading;
using log4net;
using log4net.Appender;
using log4net.Core;
using log4net.Layout;
using log4net.Repository.Hierarchy;

namespace github.trondr.LogViewer.Tests.ManualTests.FileLogTests
{
    public class TestFileLogGenerator
    {
        private readonly string _logFile;
        private bool _stop;
        // ReSharper disable once UnusedMember.Local
        private TestFileLogGenerator()
        {
        }

        public TestFileLogGenerator(string logFile)
        {
            _logFile = Environment.ExpandEnvironmentVariables(logFile);
        }

        public void Start()
        {
            _stop = false;
            var logger = ConfigureLogger();
            int i = 0;
            while(!_stop)
            {                
                i++;
                logger.Info("File test message " + i);
                Thread.Sleep(500);
            }
        }

        private ILog ConfigureLogger()
        {            
            var loggerRepository = LogManager.GetRepository();  
            loggerRepository.Threshold = Level.Debug;          
            var loggerHierarchy = (Hierarchy)loggerRepository;
            var loggerRoot = loggerHierarchy.Root;
            loggerRoot.AddAppender(GetFileAppender(_logFile));
            loggerRepository.Configured = true;
            var logger = LogManager.GetLogger(loggerRepository.Name, "tcplogger");                        
            return logger;
        }

        private static IAppender GetFileAppender(string logFile)
        {
            var appender = new RollingFileAppender
            {
                Layout = new XmlLayoutSchemaLog4j(true),
                File = Environment.ExpandEnvironmentVariables(logFile),
                ImmediateFlush = true,
                StaticLogFileName = true,
                RollingStyle = RollingFileAppender.RollingMode.Size
            };
            appender.ActivateOptions();
            return appender;
        }

        public void Stop()
        {
            _stop = true;
        }
    }
}