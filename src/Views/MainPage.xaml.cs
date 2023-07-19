using Windows.Foundation.Metadata;
using Windows.System.Profile;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace ArknightsStoryText.UWP.Views
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            if (ApiInformation.IsTypePresent(typeof(AcrylicBrush).FullName))
            {
                if (Application.Current.Resources["SystemControlAcrylicWindowBrush"] is AcrylicBrush brush)
                {
                    this.Background = brush;
                }
            }

            if (AnalyticsInfo.VersionInfo.DeviceFamily == "Windows.Mobile")
            {
                TitleBarTextBlock.Visibility = Visibility.Collapsed;
            }

            TextReadPageFrame.Navigate(typeof(TextReadPage));
            TextMergePageFrame.Navigate(typeof(TextMergePage));
        }
    }
}
