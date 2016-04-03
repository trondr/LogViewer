using System.IO;
using System.Xml;
using github.trondr.LogViewer.Library.Module.Model;

namespace github.trondr.LogViewer.Library.Module.Services
{
    public interface ILog4JLogItemParser
    {
        LogItem Parse(Stream logItemStream, string defaultLogger);

        LogItem Parse(string logItemString, string defaultLogger);

        LogItem Parse(XmlReader logItemXmlReader, string defaultLogger);
    }
}