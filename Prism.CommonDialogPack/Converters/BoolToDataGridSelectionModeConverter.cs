using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Controls;
using System.Windows.Data;

namespace Prism.CommonDialogPack.Converters
{
    public class BoolToDataGridSelectionModeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is bool canMultiselect)) throw new ArgumentException("value is not bool type");
            return canMultiselect ? DataGridSelectionMode.Extended : DataGridSelectionMode.Single;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
