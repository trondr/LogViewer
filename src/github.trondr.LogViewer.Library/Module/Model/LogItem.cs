﻿using System;
using System.Collections.Generic;

namespace github.trondr.LogViewer.Library.Module.Model
{
    public class LogItem : ILogItem
    {
        public DateTime Time { get; set; }
        public LogLevel LogLevel { get; set; }
        public string Logger { get; set; }
        public string ThreadId { get; set; }
        public string Message { get; set; }
        public string ExceptionString { get; set; }
        public Dictionary<string, string> Properties = new Dictionary<string, string>();
    }
}