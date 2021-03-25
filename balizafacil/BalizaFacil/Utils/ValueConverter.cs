using System;
using System.Globalization;
using Xamarin.Forms;

namespace BalizaFacil.Utils
{
    public delegate object ConvertDelegate(object value, Type targetType, object parameter, CultureInfo culture);
    public delegate object ConvertBasicDelegate(object value);

    public class ValueConverter : IValueConverter
    {
        private ConvertDelegate convert;
        private ConvertDelegate convertBack;

        public ValueConverter(ConvertDelegate Convert, ConvertDelegate ConvertBack = null)
        {
            this.convert = Convert;
            this.convertBack = ConvertBack;
        }

        public ValueConverter(ConvertBasicDelegate Convert, ConvertBasicDelegate ConvertBack = null)
        {
            this.convert = (value, targetType, parameter, culture) => { return Convert.Invoke(value); };
            this.convertBack = (value, targetType, parameter, culture) => { return ConvertBack?.Invoke(value); };
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                return convert.Invoke(value, targetType, parameter, culture);
            }
            catch (System.Exception ex)
            {
                string title = this.GetType().Name + " - " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                BalizaFacil.App.Instance.UnhandledException(title, ex);
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (convertBack != null)
                    return convertBack.Invoke(value, targetType, parameter, culture);
                else
                    return null;
            }
            catch (System.Exception ex)
            {
                string title = this.GetType().Name + " - " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                BalizaFacil.App.Instance.UnhandledException(title, ex);
            }

            return null;
        }
    }
}
