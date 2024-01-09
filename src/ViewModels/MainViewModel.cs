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
    }

    private void OnPivotNavigationRequested(object recipient, PivotItemIdentifier message)
    {
        View.MainPagePivot.SelectedIndex = (int)message.Index;
    }
}