using System.Globalization;
using ArknightsStoryText.UWP.ViewModels;
using Windows.Globalization.Fonts;
using Windows.System.Profile;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace ArknightsStoryText.UWP.Views
{
    /// <summary>
    /// 剧情阅读页
    /// </summary>
    public sealed partial class TextReadPage : Page
    {
        private TextReadViewModel ViewModel { get; }

        public TextReadPage()
        {
            ViewModel = new TextReadViewModel();
            this.InitializeComponent();
        }

        private void OnDoctorNameTextBoxKeyDown(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                DoctorNameFlyout.Hide();
                e.Handled = true;
            }
        }
    }
}
