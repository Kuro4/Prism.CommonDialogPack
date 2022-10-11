using Prism.CommonDialogPack.Models;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Prism.CommonDialogPack.Converters
{
    public class RGBToSolidColorBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is RGB rgb))
            {
                throw new ArgumentException("value is not RGB type.");
            }
            return new SolidColorBrush(rgb.ToMediaColor());
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is SolidColorBrush brush))
            {
                throw new ArgumentException("value is not SoliColorBrush type.");
            }
            return new RGB(brush.Color.R, brush.Color.G, brush.Color.B);
        }
    }
}
