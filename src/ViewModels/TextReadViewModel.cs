using System.IO;
using Windows.Storage.Pickers;
using Windows.Globalization.Fonts;
using ArknightsStoryText.UWP.Services;
using CommunityToolkit.Mvvm.Messaging;

namespace ArknightsStoryText.UWP.ViewModels;

public sealed partial class TextReadViewModel : ObservableRecipient
{
    [ObservableProperty]
    private string _doctorName = string.Empty;
    [ObservableProperty]
    private bool _isParagraph = false;
    [ObservableProperty]
    private bool _isLoading = false;
    [ObservableProperty]
    private ObservableCollection<StoryInfo> stories = [];

    public List<double> FontSizes { get; } = [8d, 9d, 10d, 11d, 12d, 14d, 16d, 18d, 20d, 24d, 28d, 36d, 48d, 72d];
    public IReadOnlyList<FontInfo> Fonts { get; }
    public StoryMetadataService MetadataService { get; set; } = new();
    public static FontInfo DefaultFont { get; }
    public static double DefaultFontSize => 16;

    public TextReadViewModel()
    {
        IReadOnlyList<FontInfo> fonts = FontHelper.GetSystemFonts();
        Fonts = fonts;
        IsActive = true;
    }

    static TextReadViewModel()
    {
        LanguageFontGroup languageFontGroup = new(CultureInfo.CurrentUICulture.Name);
        FontFamily defaultFont = new(languageFontGroup.ModernDocumentFont.FontFamily);
        DefaultFont = new(defaultFont.Source, defaultFont);
    }

    protected override void OnActivated()
    {
        base.OnActivated();
        WeakReferenceMessenger.Default.Register<Tuple<IEnumerable<StoryInfo>, StoryMetadataService>, string>(this, CommonValues.NotifyUpdateStoriesMessageToken, OnUpdateStories);
    }

    private async void OnUpdateStories(object recipient, Tuple<IEnumerable<StoryInfo>, StoryMetadataService> message)
    {
        MetadataService = message.Item2;
        AppendStoryInfos(message.Item1, true);
        await ReParseStoryTextAsync();
    }

    async partial void OnDoctorNameChanged(string value)
    {
        await ReParseStoryTextAsync();
    }

    async partial void OnIsParagraphChanged(bool value)
    {
        await ReParseStoryTextAsync();
    }

    [RelayCommand]
    private async Task OpenStoryTextFileAsync()
    {
        FileOpenPicker fileOpenPicker = new();
        fileOpenPicker.FileTypeFilter.Add(".txt");
        IReadOnlyList<StorageFile> files = await fileOpenPicker.PickMultipleFilesAsync();

        if (files is null || files.Count <= 0)
        {
            //用户取消了文件选择，或者文件列表为空
            return;
        }

        await ParseStoriesCore(files);
    }

    [RelayCommand]
    private async Task OpenStoryTextFolderAsync()
    {
        FolderPicker folderPicker = new();
        StorageFolder folder = await folderPicker.PickSingleFolderAsync();

        if (folder is null)
        {
            //用户取消了文件夹选择
            return;
        }

        IEnumerable<StorageFile> fileList = (await folder.GetFilesAsync()).Where(file => file.Name.EndsWith(".txt", StringComparison.OrdinalIgnoreCase));

        if (fileList.Any())
        {
            await ParseStoriesCore(fileList, true);
        }
    }

    public async Task ParseStoriesCore(IEnumerable<StorageFile> files, bool clearOriginalCollection = false)
    {
        IsLoading = true;

        if (clearOriginalCollection)
        {
            Stories.Clear();
        }

        foreach (StorageFile file in files)
        {
            await ParseOriginTextFromStorageFileAsync(file);
        }
        SortStoryList();

        IsLoading = false;
    }

    public void AppendStoryInfos(IEnumerable<StoryInfo> stories, bool clearOriginalCollection = false)
    {
        if (clearOriginalCollection)
        {
            Stories.Clear();
        }

        foreach (StoryInfo item in stories)
        {
            Stories.Add(item);
        }
    }

    [RelayCommand]
    private async Task LoadStoryMetadataAsync()
    {
        if (MetadataService.IsInitialized)
        {
            ContentDialogResult result = await ShowDialogAsync("MetadataAlreadyLoaded".GetLocalized(),
                    "ContinueOrCancel".GetLocalized(), "Continue".GetLocalized(), closeText: "Cancel".GetLocalized());

            if (result == ContentDialogResult.None)
            {
                return;
            }
        }

        FileOpenPicker fileOpenPicker = new();
        fileOpenPicker.FileTypeFilter.Add(".json");
        fileOpenPicker.CommitButtonText = "PickMetadataFileButtonText".GetLocalized();
        StorageFile file = await fileOpenPicker.PickSingleFileAsync();

        if (file is null)
        {
            //用户取消了文件选择
            return;
        }

        IsLoading = true;

        Stream utf8Json = await file.OpenStreamForReadAsync();

        if (MetadataService.TryInitialize(utf8Json))
        {
            await ReParseStoryTextAsync();
        }
        else
        {
            string title = string.Format("InvaildMetadataFile_WithPlaceholder".GetLocalized(), file.Name);
            string message = "OpenAnotherFileInstead".GetLocalized();
            await ShowDialogAsync(title, message, closeText: "OK".GetLocalized());
        }

        IsLoading = false;
    }

    [RelayCommand]
    private void RemoveSingleStoryText(object obj)
    {
        if (obj is StoryInfo info)
        {
            Stories.Remove(info);
        }
    }

    [RelayCommand]
    private void ClearStoryTexts()
    {
        IsLoading = true;
        Stories.Clear();
        IsLoading = false;
    }

    [RelayCommand]
    private static async Task OpenSettingDialog()
    {
        SettingsDialog dialog = new();
        await dialog.ShowAsync();
    }

    public async Task ReParseStoryTextAsync()
    {
        IsLoading = true;

        if (Stories.Count > 0)
        {
            StoryInfo[] _originalStoryInfos = [..Stories];
            Stories.Clear();

            for (int i = 0; i < _originalStoryInfos.Length; i++)
            {
                StoryInfo info = _originalStoryInfos[i];
                await ParseOriginTextFromStorageFileAsync(info.File);
                Stories[i] = Stories[i] with { Description = info.Description };
            }

            SortStoryList();
        }

        IsLoading = false;
    }

    private async Task<bool> ParseOriginTextFromStorageFileAsync(StorageFile file)
    {
        string storyDisplayName;
        StoryMetadataInfo? metadata;
        InfoUnlockData? detailInfo;

        if (MetadataService.TryGetMetadata(file.DisplayName, out (StoryMetadataInfo, InfoUnlockData) result))
        {
            storyDisplayName = GetStoryDisplayName(result.Item2);
            metadata = result.Item1;
            detailInfo = result.Item2;
        }
        else
        {
            storyDisplayName = file.DisplayName;
            metadata = null;
            detailInfo = null;
        }

        string text;
        try
        {
            text = await FileIO.ReadTextAsync(file);
        }
        catch (ArgumentOutOfRangeException)
        {
            string title = string.Format("InvaildFile_WithPlaceholder".GetLocalized(), file.Name);
            string message = "OpenAnotherFileInstead".GetLocalized();
            await ShowDialogAsync(title, message, closeText: "OK".GetLocalized());

            Stories.Add(new($"{storyDisplayName} [{"ParseFailed".GetLocalized()}]", $"{title}\n{message}", string.Empty, file, metadata, detailInfo));
            return false;
        }

        if (string.IsNullOrWhiteSpace(text))
        {
            string title = string.Format("FileIsEmpty_WithPlaceholder".GetLocalized(), file.Name);
            string message = "OpenAnotherFileInstead".GetLocalized();
            await ShowDialogAsync(title, message, closeText: "OK".GetLocalized());

            Stories.Add(new($"{storyDisplayName} [{"ParseFailed".GetLocalized()}]", $"{title}\n{message}", string.Empty, file, metadata, detailInfo));
            return false;
        }

        StoryReader sr = new(text, DoctorName);
        StoryScene scene;
        try
        {
            scene = sr.GetStoryScene();
        }
        catch (ArgumentException)
        {
            string title = string.Format("TutorialFileNotSupported_WithPlaceholder".GetLocalized(), file.Name);
            string message = "OpenAnotherFileInstead".GetLocalized();
            await ShowDialogAsync(title, message, closeText: "OK".GetLocalized());

            Stories.Add(new($"{storyDisplayName} [{"ParseFailed".GetLocalized()}]", $"{title}\n{message}", string.Empty, file, metadata, detailInfo));
            return false;
        }
        catch (Exception ex)
        {
            string title = string.Format("ErrorWhenParsing_WithPlaceholder".GetLocalized(), file.Name);
            string message = $"{ex.Message}\n{"OpenAnotherFileInstead".GetLocalized()}";
            await ShowDialogAsync(title, message, closeText: "OK".GetLocalized());

            Stories.Add(new($"{storyDisplayName} [{"ParseFailed".GetLocalized()}]", $"{title}\n{message}", string.Empty, file, metadata, detailInfo));
            return false;
        }

        string storyText = StoryReader.GetStoryText(scene.StoryCommands, IsParagraph);

        if (string.IsNullOrWhiteSpace(storyText))
        {
            Stories.Add(new($"{storyDisplayName} [{"ResultIsEmpty".GetLocalized()}]", storyText, string.Empty, file, metadata, detailInfo));
            return false;
        }

        Stories.Add(new(storyDisplayName, storyText, string.Empty, file, metadata, detailInfo));

        return true;
    }

    public static string GetStoryDisplayName(InfoUnlockData info)
    {
        string storyDisplayName;
        List<string> strParts = new(3);

        if (string.IsNullOrWhiteSpace(info.StoryCode) != true)
        {
            strParts.Add(info.StoryCode);
        }

        strParts.Add(info.StoryName);

        if (info.AvgTag != "幕间")
        {
            strParts.Add(info.AvgTag);
        }

        storyDisplayName = string.Join(' ', strParts);
        return storyDisplayName;
    }

    private void SortStoryList()
    {
        //TODO: 有更好的排序方式吗？
        if (MetadataService.IsInitialized)
        {
            List<StoryInfo> storyList = [..Stories];
            storyList.Sort(new StoryInfoComparer());

            if (Stories.SequenceEqual(storyList) != true)
            {
                Stories = new(storyList);
            }
            else
            {
                storyList.Clear();
                storyList = null;
            }
        }
    }

    /// <summary>
    /// 显示一个对话框
    /// </summary>
    /// <param name="title">对话框标题</param>
    /// <param name="message">要在对话框中显示的信息</param>
    /// <param name="primaryText">主按钮文本</param>
    /// <param name="secondaryText">第二按钮文本</param>
    /// <param name="closeText">关闭按钮文本</param>
    /// <returns>指示对话框结果的<seealso cref="ContentDialogResult"/></returns>
    private static async Task<ContentDialogResult> ShowDialogAsync(string title, string message, string primaryText = null, string secondaryText = null, string closeText = null)
    {
        // null-coalescing 操作符——当 closeText 为空时才赋值
        closeText ??= "Close".GetLocalized();
        primaryText ??= string.Empty;
        secondaryText ??= string.Empty;

        ContentDialog dialog = new()
        {
            Title = title,
            Content = message,
            PrimaryButtonText = primaryText,
            SecondaryButtonText = secondaryText,
            CloseButtonText = closeText
        };

        return await dialog.ShowAsync();
    }
}
