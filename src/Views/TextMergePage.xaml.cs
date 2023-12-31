﻿// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace ArknightsStoryText.UWP.Views;

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
