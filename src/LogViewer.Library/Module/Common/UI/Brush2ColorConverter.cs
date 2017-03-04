using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace LogViewer.Library.Module.Common.UI
{
    public class Brush2ColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {            
            var solidColorBrush = (SolidColorBrush)value;
            return solidColorBrush.Color;            
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {            
            var color = (Color)value;
            return new SolidColorBrush{Color = color};
        }
    }
}