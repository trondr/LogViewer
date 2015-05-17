using System;
using System.IO;
using System.Text;
using System.Xml;
using github.trondr.LogViewer.Library.Infrastructure;
using github.trondr.LogViewer.Library.Model;

namespace github.trondr.LogViewer.Library.Services
{
    
    /// <summary>
    /// Source: http://log2console.codeplex.com/SourceControl/latest#src/Log2Console/Receiver/ReceiverFactory.cs
    /// </summary>
    [Singleton]
    public class Log4JLogItemParser : ILog4JLogItemParser
    {
        static readonly DateTime S1970 = new DateTime(1970, 1, 1);
        
        public XmlReaderSettings Settings
        {
            get
            {
                if(_xmlReaderSettings == null)
                {
                    _xmlReaderSettings = new XmlReaderSettings{CloseInput = false, ValidationType = ValidationType.None};
                }
                return _xmlReaderSettings;
            }
        }
        private XmlReaderSettings _xmlReaderSettings;

        public XmlParserContext Context
        {
            get
            {
                if(_xmlParserContext == null)
                {
                    var nt = new NameTable();
                    var nsmanager = new XmlNamespaceManager(nt);
                    nsmanager.AddNamespace("log4j", "http://jakarta.apache.org/log4j/");
                    _xmlParserContext = new XmlParserContext(nt, nsmanager, "elem", XmlSpace.None, Encoding.UTF8);
                }
                return _xmlParserContext;
            }
        }
        private XmlParserContext _xmlParserContext;
        
        public LogItem Parse(Stream logItemStream, string defaultLogger)
        {
            using (var reader = XmlReader.Create(logItemStream, Settings, Context))
            return Parse(reader, defaultLogger);
        }

        public LogItem Parse(string logItemString, string defaultLogger)
        {
            try
            {
                using (var reader = new XmlTextReader(logItemString, XmlNodeType.Element, Context))
                    return Parse(reader, defaultLogger);
            }
            catch (Exception e)
            {
                return new LogItem()
                {
                    // Create a simple log message with some default values
                    Logger = defaultLogger,
                    ThreadId = "NA",
                    Message = logItemString,
                    Time = DateTime.Now,
                    LogLevel = LogLevel.Info,
                    ExceptionString = e.Message
                };
            }
        }

        public LogItem Parse(XmlReader logItemXmlReader, string defaultLogger)
        {
            var logMsg = new LogItem();

            logItemXmlReader.Read();
            if ((logItemXmlReader.MoveToContent() != XmlNodeType.Element) || (logItemXmlReader.Name != "log4j:event"))
            { 
                throw new Exception("The Log Event is not a valid log4j Xml block.");
            }
            logMsg.Logger = logItemXmlReader.GetAttribute("logger");
            var logLevelString = logItemXmlReader.GetAttribute("level");
            var logLevel = LogLevel.None;
            var sucessFullConversion = Enum.TryParse<LogLevel>(logLevelString, out logLevel);
            logMsg.LogLevel = logLevel;
            logMsg.ThreadId = logItemXmlReader.GetAttribute("thread");
            long timeStamp;
            if (long.TryParse(logItemXmlReader.GetAttribute("timestamp"), out timeStamp))
                logMsg.Time = S1970.AddMilliseconds(timeStamp).ToLocalTime();

            var eventDepth = logItemXmlReader.Depth;
            logItemXmlReader.Read();
            while (logItemXmlReader.Depth > eventDepth)
            {
                if (logItemXmlReader.MoveToContent() == XmlNodeType.Element)
                {
                    switch (logItemXmlReader.Name)
                    {
                        case "log4j:message":
                            logMsg.Message = logItemXmlReader.ReadString();
                            break;

                        case "log4j:throwable":
                            logMsg.Message += Environment.NewLine + logItemXmlReader.ReadString();
                            break;

                        case "log4j:locationInfo":
                            break;

                        case "log4j:properties":
                            logItemXmlReader.Read();
                            while (logItemXmlReader.MoveToContent() == XmlNodeType.Element
                                   && logItemXmlReader.Name == "log4j:data")
                            {
                                var name = logItemXmlReader.GetAttribute("name");
                                var value = logItemXmlReader.GetAttribute("value");
                                logMsg.Properties[name] = value;
                                logItemXmlReader.Read();
                            }
                            break;
                    }
                }
                logItemXmlReader.Read();
            }
            return logMsg;
        }
    }
}