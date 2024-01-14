// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace ArknightsStoryText.UWP.Views;

/// <summary>
/// 可用于自身或导航至 Frame 内部的空白页。
/// </summary>
public sealed partial class MainPage : Page
{
    public MainViewModel ViewModel { get; }

    public MainPage()
    {
        this.InitializeComponent();

        if (MicaHelper.IsSupported())
        {
            MicaHelper.TrySetMica(this);
        }
        else if (AcrylicHelper.IsSupported())
        {
            AcrylicHelper.TrySetAcrylicBrush(this);
        }

        if (EnvironmentHelper.IsWindowsMobile)
        {
            TitleBarTextBlock.Visibility = Visibility.Collapsed;
        }

        TextReadPageFrame.Navigate(typeof(TextReadPage));
        TextMergePageFrame.Navigate(typeof(TextMergePage));
        StoryGlancePageFrame.Navigate(typeof(StoryGlancePage));

        ViewModel = new(this);
    }
}
