using IoTDemo.Infrastructure;
using IoTDemo.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace IoTDemo.Converters
{
    public class BoolToColorConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string[] colors = parameter.ToString().Split('|');
            if (System.Convert.ToBoolean(value))
            {
                return Color.FromHex(colors[0]);
            }
            else
            {
                return Color.FromHex(colors[1]);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
