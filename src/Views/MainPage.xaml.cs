using ArknightsResources.Stories.Models;
using ArknightsResources.Utility;
using ArknightsStoryText.UWP.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Foundation.Metadata;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.System.Profile;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

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
        }
    }
}
