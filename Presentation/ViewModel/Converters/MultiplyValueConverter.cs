using System.Globalization;
using System.Windows.Data;

namespace Presentation.ViewModel.Converters
{
    internal class MultiplyValueConverter : IValueConverter
    {
        public double Multiplier { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                return System.Convert.ToDouble(value) * Multiplier;
            }
            catch (Exception)
            {
                return double.NaN;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                return System.Convert.ToDouble(value) / Multiplier;
            }
            catch (Exception)
            {
                return double.NaN;
            }
        }
    }
}
