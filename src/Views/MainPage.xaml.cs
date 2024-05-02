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

        AutoSetMainPageBackground();

        if (EnvironmentHelper.IsWindowsMobile)
        {
            TitleBarTextBlock.Visibility = Visibility.Collapsed;
        }

        TextReadPageFrame.Navigate(typeof(TextReadPage));
        TextMergePageFrame.Navigate(typeof(TextMergePage));
        StoryGlancePageFrame.Navigate(typeof(StoryGlancePage));

        ViewModel = new(this);
    }

    private void AutoSetMainPageBackground()
    {
        if (SettingsHelper.TryGet(CommonValues.AppBackgroundModeSettingsKey, out string modeString) && Enum.TryParse(modeString, out AppBackgroundMode mode))
        {
            // :D
        }
        else
        {
            if (MicaHelper.IsSupported())
            {
                mode = AppBackgroundMode.Mica;
            }
            else if (AcrylicHelper.IsSupported())
            {
                mode = AppBackgroundMode.Acrylic;
            }
            else
            {
                mode = AppBackgroundMode.PureColor;
            }
        }

        SetMainPageBackground(mode);
    }

    public bool SetMainPageBackground(AppBackgroundMode mode)
    {
        switch (mode)
        {
            case AppBackgroundMode.Acrylic:
                return AcrylicHelper.TrySetAcrylicBrush(this);
            case AppBackgroundMode.Mica:
                // 设置 Mica 时，要将控件背景设置为透明
                Background = new SolidColorBrush(Colors.Transparent);
                return MicaHelper.TrySetMica(this);
            case AppBackgroundMode.PureColor:
            default:
                Background = Resources["ApplicationPageBackgroundThemeBrush"] as Brush;
                return true;
        }
    }
}
