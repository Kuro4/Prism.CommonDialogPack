using System;
using System.Globalization;
using System.Windows.Data;

namespace Prism.CommonDialogPack.Converters
{
    public class ByteSizeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is null) return string.Empty;
            if (!(value is long byteValue)) throw new ArgumentException("value is not long? type");

            string[] units = {" ", "K", "M", "G", "T" };
            double convertedValue = byteValue;
            int index = 0;
            while(1024 <= convertedValue && index < units.Length - 1)
            {
                convertedValue /= 1024;
                index++;
            }
            return $"{convertedValue:F2}{units[index]}B";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
