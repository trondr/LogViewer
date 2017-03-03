using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using LogViewer.Library.Module.Model;
using LogLevel = LogViewer.Library.Module.Model.LogLevel;

namespace LogViewer.Library.Module.Services.WinDebugLog
{
    public class WinDebugLogItemHandler : ILogItemHandler<WinDebugLogItemConnection>
    {
        public ILogItemConnection Connection { get; set; }
        private ILogItemNotifiable _logItemNotifiable;
        private static readonly List<LogLevel> LogLevels = Enum.GetValues(typeof(LogLevel)).Cast<LogLevel>().ToList();
        private static Random _random = new Random();

        
        public void Initialize()
        {
            DebugMonitor.OnOutputDebugString += DebugMonitorOnOutputDebugString;
            DebugMonitor.Start();
        }

        public void Terminate()
        {
            DebugMonitor.OnOutputDebugString -= DebugMonitorOnOutputDebugString;
            DebugMonitor.Stop();
        }

        void DebugMonitorOnOutputDebugString(int pid, string text)
        {
            // Trim ending newline (if any) 
            if (text.EndsWith(Environment.NewLine))
                text = text.Substring(0, text.Length - Environment.NewLine.Length);

            // Replace dots by "middle dots" to preserve Logger namespace
            string processName = GetProcessName(pid);
            processName = processName.Replace('.', '·');

            var logItem = new LogItem
            {
                Message = text,
                Logger = string.Format("{0}.{1}", processName, pid),
                LogLevel = LogLevel.Debug,
                ThreadId = pid.ToString(),
                Time = DateTime.Now
            };
            _logItemNotifiable?.Notify(logItem);
        }

        private static string GetProcessName(int pid)
        {
            if (pid == -1)
                return Process.GetCurrentProcess().ProcessName;
            try
            {
                return Process.GetProcessById(pid).ProcessName;
            }
            catch
            {
                return "<exited>";
            }
        }
        
        public void Attach(ILogItemNotifiable logItemNotifiable)
        {
            _logItemNotifiable = logItemNotifiable;
        }

        public void Detach()
        {
            _logItemNotifiable = null;
        }

        public bool ShowFromBeginning { get; set; }

        public string DefaultLoggerName { get; set; }
    }
}