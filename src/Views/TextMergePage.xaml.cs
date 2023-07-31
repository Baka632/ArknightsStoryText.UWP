﻿using ArknightsStoryText.UWP.ViewModels;
using Windows.System.Profile;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace ArknightsStoryText.UWP.Views
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class TextMergePage : Page
    {
        public TextMergeViewModel ViewModel { get; }

        public TextMergePage()
        {
            ViewModel = new TextMergeViewModel();
            this.InitializeComponent();

            if (AnalyticsInfo.VersionInfo.DeviceFamily != "Windows.Mobile")
            {
                OperationCommandBar.Background = new SolidColorBrush(Colors.Transparent);
            }
        }
    }
}