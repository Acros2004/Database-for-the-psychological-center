using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows;
using CleanBrain.ManagerFrame;

namespace CleanBrain.Converter
{
    public class VisButtonConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            int id_revClient = (int)values[0];
            if (!ManagerItem.ImClient)
                return Visibility.Visible;
            else
            {
                if(id_revClient == ManagerItem.MainId)
                    return Visibility.Visible;
                else
                    return Visibility.Collapsed;
            }
        }
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
