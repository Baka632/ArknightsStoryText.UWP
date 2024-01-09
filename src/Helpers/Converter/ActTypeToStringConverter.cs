using Windows.UI.Xaml.Data;

namespace ArknightsStoryText.UWP.Helpers.Converter;

public sealed class ActTypeToStringConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is ActType actType)
        {
            return actType switch
            {
                ActType.MainStory => "MainStory".GetLocalized(),
                ActType.Activity => "ActivityStory".GetLocalized(),
                ActType.MiniStory => "MiniStory".GetLocalized(),
                ActType.Others => "OtherStory".GetLocalized(),
                _ => throw new NotImplementedException(),
            };
        }
        else
        {
            return DependencyProperty.UnsetValue;
        }
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}
