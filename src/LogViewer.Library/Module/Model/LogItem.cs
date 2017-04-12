using System;
using System.Collections.Generic;

namespace LogViewer.Library.Module.Model
{
    public class LogItem : ILogItem
    {
        public DateTime Time { get; set; }
        public LogLevel LogLevel { get; set; }
        public string Logger { get; set; }
        public string ThreadId { get; set; }
        public string Message { get; set; }
        public string ExceptionString { get; set; }
        public string CallSiteClass { get; set; }
        public string CallSiteMethod { get; set; }
        public string SourceFileName { get; set; }
        public uint SourceFileLineNr { get; set; }
        public Dictionary<string, string> Properties = new Dictionary<string, string>();
    }
}