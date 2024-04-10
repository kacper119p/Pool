using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Presentation.ViewModel.Converters
{
    internal class ColorToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType != typeof(Brush)) { return Brushes.Magenta; }
            if (value is not System.Drawing.Color color) { return Brushes.Magenta; }
            return new SolidColorBrush(Color.FromRgb(color.R, color.G, color.B));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
