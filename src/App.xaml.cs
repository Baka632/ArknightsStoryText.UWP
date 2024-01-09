using System.IO;
using Microsoft.UI.Xaml.Controls;
using Microsoft.Toolkit.Uwp.Notifications;
using Windows.System.Profile;
using Windows.UI.Xaml.Navigation;
using Windows.UI.ViewManagement;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Core;
using Windows.UI.Notifications;

namespace ArknightsStoryText.UWP;

/// <summary>
/// 提供特定于应用程序的行为，以补充默认的应用程序类。
/// </summary>
sealed partial class App : Application
{
    public static bool IsGreaterThan18362 => EnvironmentHelper.IsSystemBuildVersionEqualOrGreaterThan(18362);

    /// <summary>
    /// 初始化单一实例应用程序对象。这是执行的创作代码的第一行，已执行，逻辑上等同于 main() 或 WinMain()。
    /// </summary>
    public App()
    {
        this.InitializeComponent();

        UnhandledException += OnUnhandledException;
        this.Suspending += OnSuspending;
    }

    private async void OnUnhandledException(object sender, Windows.UI.Xaml.UnhandledExceptionEventArgs e)
    {
        e.Handled = true;

        Exception exception = e.Exception;
        ToastContent toastContent = new()
        {
            Visual = new ToastVisual()
            {
                BindingGeneric = new ToastBindingGeneric()
                {
                    Children =
                    {
                        new AdaptiveText()
                        {
                            Text = exception?.GetType()?.Name
                        },
                        new AdaptiveText()
                        {
                            Text = exception?.Message
                        },
                        new AdaptiveText()
                        {
                            Text = exception?.StackTrace
                        }
                    }
                }
            }
        };

        ToastNotification toastNotif = new(toastContent.GetXml());
        ToastNotificationManager.CreateToastNotifier().Show(toastNotif);

        StorageFolder temporaryFolder = ApplicationData.Current.TemporaryFolder;
        StorageFolder logFolder = await temporaryFolder.CreateFolderAsync("Log", CreationCollisionOption.OpenIfExists);
        StorageFile logFile = await logFolder.CreateFileAsync($"Log-{DateTimeOffset.UtcNow:yyyy-MM-dd HH,mm,ss.fff}.log");

        using StorageStreamTransaction transaction = await logFile.OpenTransactedWriteAsync();
        using Stream target = transaction.Stream.AsStreamForWrite();
        target.Seek(0, SeekOrigin.Begin);

        using StreamWriter writer = new(target);
        await writer.WriteAsync($"""
            [Exception Detail]
            {exception?.GetType()?.Name}: {exception?.Message}
            ======
            Source: {exception?.Source}
            HResult: {exception?.HResult}
            TargetSite Info:
                Name: {exception?.TargetSite.Name}
                Module Name: {exception?.TargetSite?.Module.Name}
                DeclaringType: {exception?.TargetSite?.DeclaringType.Name}
            StackTrace:
            {exception?.StackTrace}
            """);
        await writer.FlushAsync();

        await transaction.CommitAsync();
    }

    /// <summary>
    /// 在应用程序由最终用户正常启动时进行调用。
    /// 将在启动应用程序以打开特定文件等情况下使用。
    /// </summary>
    /// <param name="e">有关启动请求和过程的详细信息。</param>
    protected override void OnLaunched(LaunchActivatedEventArgs e)
    {
#if DEBUG
        //调试本地化的相关代码

        //Windows.Globalization.ApplicationLanguages.PrimaryLanguageOverride = "en-US";
        //Windows.Globalization.ApplicationLanguages.PrimaryLanguageOverride = "zh-CN";
#endif


        // 不要在窗口已包含内容时重复应用程序初始化，只需确保窗口处于活动状态
        if (Window.Current.Content is not Frame rootFrame)
        {
            // 创建要充当导航上下文的框架，并导航到第一页
            rootFrame = new Frame();

            rootFrame.NavigationFailed += OnNavigationFailed;

            if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
            {
                //TODO: 从之前挂起的应用程序加载状态
            }

            // 将框架放在当前窗口中
            Window.Current.Content = rootFrame;
            XamlControlsResources muxcStyle = new()
            {
                ControlsResourcesVersion = Environment.OSVersion.Version.Build >= 22000
                ? ControlsResourcesVersion.Version2
                : ControlsResourcesVersion.Version1
            };
            Resources.MergedDictionaries.Add(muxcStyle);
            SetTitleBar();
        }

        if (e.PrelaunchActivated == false)
        {
            if (rootFrame.Content == null)
            {
                // 当导航堆栈尚未还原时，导航到第一页，并通过将所需信息作为导航参数传入来配置参数
                rootFrame.Navigate(typeof(MainPage), e.Arguments);
            }
            // 确保当前窗口处于活动状态
            Window.Current.Activate();
        }
    }

    private static void SetTitleBar()
    {
        ApplicationViewTitleBar PresentationTitleBar = ApplicationView.GetForCurrentView().TitleBar;
        PresentationTitleBar.ButtonBackgroundColor = Colors.Transparent;
        CoreApplicationViewTitleBar coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
        coreTitleBar.ExtendViewIntoTitleBar = AnalyticsInfo.VersionInfo.DeviceFamily != "Windows.Mobile";

        Color ForegroundColor = Current.RequestedTheme switch
        {
            ApplicationTheme.Light => Colors.Black,
            ApplicationTheme.Dark => Colors.White,
            _ => Colors.White,
        };
        PresentationTitleBar.ButtonForegroundColor = ForegroundColor;
    }

    /// <summary>
    /// 导航到特定页失败时调用
    /// </summary>
    ///<param name="sender">导航失败的框架</param>
    ///<param name="e">有关导航失败的详细信息</param>
    void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
    {
        throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
    }

    /// <summary>
    /// 在将要挂起应用程序执行时调用。  在不知道应用程序
    /// 无需知道应用程序会被终止还是会恢复，
    /// 并让内存内容保持不变。
    /// </summary>
    /// <param name="sender">挂起的请求的源。</param>
    /// <param name="e">有关挂起请求的详细信息。</param>
    private void OnSuspending(object sender, SuspendingEventArgs e)
    {
        SuspendingDeferral deferral = e.SuspendingOperation.GetDeferral();
        //TODO: 保存应用程序状态并停止任何后台活动
        deferral.Complete();
    }
}
