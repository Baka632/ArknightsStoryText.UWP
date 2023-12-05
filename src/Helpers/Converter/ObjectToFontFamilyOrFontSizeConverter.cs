using Windows.UI.Xaml.Data;

namespace ArknightsStoryText.UWP.Helpers.Converter
{
    public class ObjectToFontFamilyOrDoubleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return value switch
            {
                FontFamily fontFamily => fontFamily,
                double fontSize => fontSize,
                _ => DependencyProperty.UnsetValue
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
