using System;
using System.Windows.Media;
using github.trondr.LogViewer.Library.Module.Model;

namespace github.trondr.LogViewer.Library.Module.ViewModels
{
    public class LogLevelSettings : ILogLevelSettings
    {
        public SolidColorBrush LoadForegroundColor(LogLevel logLevel)
        {
            switch (logLevel)
            {
                case LogLevel.Trace:
                    return new SolidColorBrush {Color = Properties.Settings.Default.TraceColor};
                case LogLevel.Debug:
                    return new SolidColorBrush {Color = Properties.Settings.Default.DebugColor};
                case LogLevel.Info:
                    return new SolidColorBrush {Color = Properties.Settings.Default.InfoColor};
                case LogLevel.Warn:
                    return new SolidColorBrush {Color = Properties.Settings.Default.WarnColor};
                case LogLevel.Error:
                    return new SolidColorBrush {Color = Properties.Settings.Default.ErrorColor};
                case LogLevel.Fatal:
                    return new SolidColorBrush {Color = Properties.Settings.Default.FatalColor};
                default:
                    throw new ArgumentOutOfRangeException("logLevel");
            }
        }

        public void SaveForegroundColor(LogLevel logLevel, SolidColorBrush color)
        {
            switch (logLevel)
            {
                case LogLevel.Trace:
                    Properties.Settings.Default.TraceColor = color.Color;
                    break;
                case LogLevel.Debug:
                    Properties.Settings.Default.DebugColor = color.Color;
                    break;
                case LogLevel.Info:
                    Properties.Settings.Default.InfoColor = color.Color;
                    break;
                case LogLevel.Warn:
                    Properties.Settings.Default.WarnColor = color.Color;
                    break;
                case LogLevel.Error:
                    Properties.Settings.Default.ErrorColor = color.Color;
                    break;
                case LogLevel.Fatal:
                    Properties.Settings.Default.FatalColor = color.Color;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("logLevel");
            }
        }
    }
}