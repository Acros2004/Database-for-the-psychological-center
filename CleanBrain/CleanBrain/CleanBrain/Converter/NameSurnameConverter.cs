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
    public class NameSurnameConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            int id = (int)values[0];
            UnitOfWork unit = new UnitOfWork();
            Psychologist psy = unit.Psychologist.Get(id);


            return $"{psy.Surname_Psychologist} {psy.Name_Psychologist}";
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
