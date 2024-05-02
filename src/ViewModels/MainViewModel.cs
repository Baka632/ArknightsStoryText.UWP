using CommunityToolkit.Mvvm.Messaging;

namespace ArknightsStoryText.UWP.ViewModels;

public class MainViewModel : ObservableRecipient
{
    public MainViewModel(MainPage mainPage)
    {
        View = mainPage;
        IsActive = true;
    }

    public MainPage View { get; }

    protected override void OnActivated()
    {
        base.OnActivated();
        WeakReferenceMessenger.Default.Register<PivotItemIdentifier, string>(this, CommonValues.NotifyPivotNavigationMessageToken, OnPivotNavigationRequested);
        WeakReferenceMessenger.Default.Register<string, string>(this, CommonValues.NotifyAppBackgroundChangedMessageToken, OnAppBackgroundChanged);
    }

    private void OnPivotNavigationRequested(object recipient, PivotItemIdentifier message)
    {
        View.MainPagePivot.SelectedIndex = (int)message.Index;
    }

    private void OnAppBackgroundChanged(object recipient, string message)
    {
        if (Enum.TryParse(message, out AppBackgroundMode mode))
        {
            View.SetMainPageBackground(mode);
        }
    }
}