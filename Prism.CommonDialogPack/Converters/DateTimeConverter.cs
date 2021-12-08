using System;
using System.Globalization;
using System.Windows.Data;

namespace Prism.CommonDialogPack.Converters
{
    public class DateTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is DateTime dateTime)) throw new ArgumentException("value is not DateTime type");
            return dateTime.ToString("yyyy/MM/dd H:mm");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
