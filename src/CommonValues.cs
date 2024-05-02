using Windows.ApplicationModel;

namespace ArknightsStoryText.UWP;

internal static class CommonValues
{
    public const string NotifyUpdateStoriesMessageToken = "Notify_UpdateStories_MessageToken";
    public const string NotifyUpdateStoryFileInfosMessageToken = "Notify_UpdateStoryFileInfos_MessageToken";
    public const string NotifyPivotNavigationMessageToken = "Notify_PivotNavigation_MessageToken";
    public const string AppBackgroundModeSettingsKey = "App_BackgroundMode_SettingsKey";
    public const string NotifyAppBackgroundChangedMessageToken = "Notify_AppBackgroundChanged_MessageToken";

    /// <summary>
    /// 获取应用程序版本
    /// </summary>
    public static string AppVersion => $"{Package.Current.Id.Version.Major}.{Package.Current.Id.Version.Minor}.{Package.Current.Id.Version.Build}.{Package.Current.Id.Version.Revision}";
    /// <summary>
    /// 获取带“版本”文字的应用程序版本字符串
    /// </summary>
    public static string AppVersionWithText => string.Format("AppVersion_WithPlaceholder".GetLocalized(), AppVersion);
}
