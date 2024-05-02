// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“内容对话框”项模板

namespace ArknightsStoryText.UWP.Views;

public sealed partial class SettingsDialog : ContentDialog
{
    public SettingsViewModel ViewModel { get; } = new SettingsViewModel();

    public SettingsDialog()
    {
        this.InitializeComponent();
    }
}
