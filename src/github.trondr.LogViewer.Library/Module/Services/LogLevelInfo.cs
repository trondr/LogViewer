using System;
using github.trondr.LogViewer.Library.Module.Model;

namespace github.trondr.LogViewer.Library.Module.Services
{
    [Serializable]
    public class LogLevelInfo
    {
        public LogLevel Level { get; }
        public int Value { get; }
        public int RangeMin { get; }
        public int RangeMax { get; }

        public LogLevelInfo(LogLevel level, int value, int rangeMin, int rangeMax)
        {
            Level = level;
            Value = value;
            RangeMin = rangeMin;
            RangeMax = rangeMax;
        }

        public override bool Equals(object obj)
        {
            var info = obj as LogLevelInfo;
            if (info != null)
                return (info == this);
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public static bool operator ==(LogLevelInfo first, LogLevelInfo second)
        {
            if (first == null) throw new ArgumentNullException(nameof(first));
            if (second == null) throw new ArgumentNullException(nameof(second));
            return (first.Value == second.Value);
        }

        public static bool operator !=(LogLevelInfo first, LogLevelInfo second)
        {
            if (first == null) throw new ArgumentNullException(nameof(first));
            if (second == null) throw new ArgumentNullException(nameof(second));
            return (first.Value != second.Value);
        }
    }
}