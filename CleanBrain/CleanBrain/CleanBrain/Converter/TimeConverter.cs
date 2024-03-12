using CleanBrain.UoF;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace CleanBrain.Converter
{
    public class TimeConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            int id = (int)values[0];
            UnitOfWork unit = new UnitOfWork();
            Voucher psy = unit.Voucher.Get(id);
            string first = psy.Time_Voucher_Start.ToString().Substring(0, 5);
            string last = psy.Time_Voucher_End.ToString().Substring(0, 5);
            return $"{first}-{last}";
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
