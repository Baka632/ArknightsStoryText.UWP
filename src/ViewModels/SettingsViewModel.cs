using CommunityToolkit.Mvvm.Messaging;

namespace ArknightsStoryText.UWP.ViewModels;

public sealed partial class SettingsViewModel : ObservableObject
{
    public readonly IReadOnlyList<AppBackgroundMode> AppBackgroundModes;

    [ObservableProperty]
    private int selectedAppBackgroundModeIndex;

    public SettingsViewModel()
    {
        List<AppBackgroundMode> bgModes = new(3);

        bool isSupportMica = MicaHelper.IsSupported();
        bool isSupportAcrylic = AcrylicHelper.IsSupported();

        if (isSupportMica)
        {
            bgModes.Add(AppBackgroundMode.Mica);
        }

        if (isSupportAcrylic)
        {
            bgModes.Add(AppBackgroundMode.Acrylic);
        }

        // 不管什么情况，系统一定支持纯色背景显示
        bgModes.Add(AppBackgroundMode.PureColor);

        AppBackgroundModes = bgModes;

        if (SettingsHelper.TryGet(CommonValues.AppBackgroundModeSettingsKey, out string bgModeString) && Enum.TryParse(bgModeString, out AppBackgroundMode backgroundMode))
        {
            // ;-)
        }
        else
        {
            if (isSupportMica)
            {
                backgroundMode = AppBackgroundMode.Mica;
            }
            else if (isSupportAcrylic)
            {
                backgroundMode = AppBackgroundMode.Acrylic;
            }
            else
            {
                backgroundMode = AppBackgroundMode.PureColor;
            }
        }

        selectedAppBackgroundModeIndex = bgModes.IndexOf(backgroundMode);
    }

    partial void OnSelectedAppBackgroundModeIndexChanged(int value)
    {
        if (value >= 0)
        {
            AppBackgroundMode bgMode = AppBackgroundModes[value];
            string bgModeString = bgMode.ToString();
            SettingsHelper.Set(CommonValues.AppBackgroundModeSettingsKey, bgModeString);

            WeakReferenceMessenger.Default.Send(bgModeString, CommonValues.NotifyAppBackgroundChangedMessageToken);
        }
    }
}