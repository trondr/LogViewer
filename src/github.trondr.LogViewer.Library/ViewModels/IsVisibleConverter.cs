using System;
using System.Globalization;
using System.Windows.Data;

namespace github.trondr.LogViewer.Library.ViewModels
{
    public class IsVisibleConverter: IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            Console.Write("v");
            var isVisible1 = (bool)values[0];
            var isVisible2 = (bool)values[1];
            return isVisible1 && isVisible2;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
