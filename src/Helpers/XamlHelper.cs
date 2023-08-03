using Windows.UI.Xaml;

namespace ArknightsStoryText.UWP.Helpers;

public static class XamlHelper
{
    public static bool ReverseBoolean(bool value) => !value;

    public static Visibility ReverseVisibility(Visibility value)
    {
        return value switch
        {
            Visibility.Visible => Visibility.Collapsed,
            _ => Visibility.Visible,
        };
    }
    
    public static Visibility ReverseVisibility(bool value)
    {
        return value switch
        {
            true => Visibility.Collapsed,
            false => Visibility.Visible,
        };
    }
}
