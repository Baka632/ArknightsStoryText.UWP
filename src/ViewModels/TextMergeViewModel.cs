using System.IO;
using ArknightsStoryText.UWP.Services;
using Windows.Storage.Pickers;

namespace ArknightsStoryText.UWP.ViewModels;

public partial class TextMergeViewModel : ObservableObject
{
    private readonly StoryMetadataService metadataService = new();
    private static readonly string[] defaultFileExtensions = [".txt"];

    [ObservableProperty]
    private string _transformedStoryText = string.Empty;
    [ObservableProperty]
    private string _doctorName = string.Empty;
    [ObservableProperty]
    private bool _isParagraph = false;
    [ObservableProperty]
    private bool _isMerging = false;
    [ObservableProperty]
    private ObservableCollection<StoryFileInfo> files = [];

    [RelayCommand]
    private async Task OpenStoryTextFileAsync()
    {
        FileOpenPicker fileOpenPicker = new();
        fileOpenPicker.FileTypeFilter.Add(".txt");
        IReadOnlyList<StorageFile> storageFiles = await fileOpenPicker.PickMultipleFilesAsync();

        if (storageFiles is not null && storageFiles.Count > 0)
        {
            foreach (StorageFile file in storageFiles)
            {
                string storyDisplayName;
                StoryMetadataInfo? metadata;
                InfoUnlockData? detailInfo;

                if (metadataService.TryGetMetadata(file.DisplayName, out (StoryMetadataInfo, InfoUnlockData) result))
                {
                    InfoUnlockData item2 = result.Item2;
                    List<string> strParts = new(3);

                    if (string.IsNullOrWhiteSpace(item2.StoryCode) != true)
                    {
                        strParts.Add(item2.StoryCode);
                    }

                    strParts.Add(item2.StoryName);

                    if (item2.AvgTag != "幕间")
                    {
                        strParts.Add(item2.AvgTag);
                    }

                    storyDisplayName = string.Join(' ', strParts);

                    metadata = result.Item1;
                    detailInfo = result.Item2;
                }
                else
                {
                    storyDisplayName = string.Empty;
                    metadata = null;
                    detailInfo = null;
                }

                if (!Files.Any(fileInfo => fileInfo.File.Path == file.Path))
                {
                    Files.Add(new StoryFileInfo(file, storyDisplayName, metadata, detailInfo));
                }
            }

            SortFileList();
        }
    }

    [RelayCommand]
    private async Task SaveStoryTextFileAsync()
    {
        StringBuilder stringBuilder = new(20);
        int count = 0;

        if (!Files.Any())
        {
            await ShowDialogAsync("NoFileImported".GetLocalized(), "AddSomeFiles".GetLocalized(), closeText:"Close".GetLocalized());
            return;
        }

        foreach (StoryFileInfo item in Files)
        {
            string text;
            try
            {
                text = await FileIO.ReadTextAsync(item.File);
            }
            catch (ArgumentOutOfRangeException)
            {
                string title = string.Format("InvaildFile_WithPlaceholder".GetLocalized(), item.File.Name);
                ContentDialogResult result = await ShowDialogAsync(title,
                    "ContinueOrCancel".GetLocalized(), "Continue".GetLocalized(), closeText: "Cancel".GetLocalized());

                if (result == ContentDialogResult.None)
                {
                    stringBuilder.Clear();
                    return;
                }
                else
                {
                    continue;
                }
            }

            if (string.IsNullOrWhiteSpace(text))
            {
                string title = string.Format("FileIsEmpty_WithPlaceholder".GetLocalized(), item.File.Name);
                ContentDialogResult result = await ShowDialogAsync(title,
                    "ContinueOrCancel".GetLocalized(), "Continue".GetLocalized(), closeText: "Cancel".GetLocalized());

                if (result == ContentDialogResult.None)
                {
                    stringBuilder.Clear();
                    return;
                }
                else
                {
                    continue;
                }
            }

            StoryReader sr = new(text, DoctorName);
            StoryScene scene;
            try
            {
                scene = sr.GetStoryScene();
            }
            catch (ArgumentException)
            {
                string title = string.Format("TutorialFileNotSupported_WithPlaceholder".GetLocalized(),item.File.Name);
                ContentDialogResult result = await ShowDialogAsync(title,
                    "ContinueOrCancel".GetLocalized(), "Continue".GetLocalized(), closeText: "Cancel".GetLocalized());

                if (result == ContentDialogResult.None)
                {
                    stringBuilder.Clear();
                    return;
                }
                else
                {
                    continue;
                }
            }
            catch (Exception ex)
            {
                string title = string.Format("ErrorWhenParsing_WithPlaceholder".GetLocalized(), item.File.Name);
                ContentDialogResult result = await ShowDialogAsync(title,
                    $"{ex.Message}\n{"ContinueOrCancel".GetLocalized()}", "Continue".GetLocalized(), closeText: "Cancel".GetLocalized());

                if (result == ContentDialogResult.None)
                {
                    stringBuilder.Clear();
                    return;
                }
                else
                {
                    continue;
                }
            }

            string storyText = StoryReader.GetStoryText(scene.StoryCommands, IsParagraph);

            //TODO: 自定义
            count++;

            stringBuilder.AppendLine($"{count}. {item.Title}");
            stringBuilder.AppendLine();
            stringBuilder.AppendLine(storyText);
            stringBuilder.AppendLine();

        }

        IEnumerable<string> names = (from info in Files
                                     where info.MetadataInfo.HasValue
                                     select info.MetadataInfo.Value.Name).Distinct();
        int namesCount = names.Count();

        FileSavePicker fileSavePicker = new();
        fileSavePicker.FileTypeChoices.Add("StoryTxtFileDescription".GetLocalized(), defaultFileExtensions);

        if (namesCount == 1)
        {
            fileSavePicker.SuggestedFileName = names.First();
        }

        StorageFile saveFile = await fileSavePicker.PickSaveFileAsync();

        if (saveFile is not null)
        {
            await FileIO.WriteTextAsync(saveFile, stringBuilder.ToString());
            string fileSaveLocationTip = string.Format("FileSavePathTip_WithPlaceholder".GetLocalized(), saveFile.Path);
            await ShowDialogAsync("FileSaveComplete".GetLocalized(), fileSaveLocationTip, closeText: "OK".GetLocalized());
        }
    }

    [RelayCommand]
    private async Task LoadStoryMetadataAsync()
    {
        if (metadataService.IsInitialized)
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

        Stream utf8Json = await file.OpenStreamForReadAsync();

        if (metadataService.TryInitialize(utf8Json))
        {
            for (int i = 0; i < Files.Count; i++)
            {
                StoryFileInfo info = Files[i];
                if (metadataService.TryGetMetadata(info.File.DisplayName, out (StoryMetadataInfo, InfoUnlockData) result))
                {
                    info.MetadataInfo = result.Item1;
                    info.DetailInfo = result.Item2;

                    List<string> strParts = new(3);

                    if (string.IsNullOrWhiteSpace(result.Item2.StoryCode) != true)
                    {
                        strParts.Add(result.Item2.StoryCode);
                    }

                    strParts.Add(result.Item2.StoryName);

                    if (result.Item2.AvgTag != "幕间")
                    {
                        strParts.Add(result.Item2.AvgTag);
                    }

                    info.Title = string.Join(' ', strParts);
                }
            }
            SortFileList();
        }
        else
        {
            string title = string.Format("InvaildMetadataFile_WithPlaceholder".GetLocalized(), file.Name);
            string message = "OpenAnotherFileInstead".GetLocalized();
            await ShowDialogAsync(title, message, closeText: "OK".GetLocalized());
        }
    }

    [RelayCommand]
    private void ClearStoryTexts()
    {
        Files.Clear();
    }

    [RelayCommand]
    private void RemoveStoryTextFile(object obj)
    {
        if (obj is StoryFileInfo fileInfo)
        {
            Files.Remove(fileInfo);
        }
    }

    private void SortFileList()
    {
        //TODO: 有更好的排序方式吗？
        if (metadataService.IsInitialized)
        {
            List<StoryFileInfo> fileList = [..Files];
            fileList.Sort(CompareStoryInfo);

            if (Files.SequenceEqual(fileList) != true)
            {
                Files = new(fileList);
            }
            else
            {
                fileList.Clear();
                fileList = null;
            }
        }
    }

    private int CompareStoryInfo(StoryFileInfo x, StoryFileInfo y)
    {
        if (x.MetadataInfo.HasValue && y.MetadataInfo.HasValue != true)
        {
            return 1;
        }
        else if (x.MetadataInfo.HasValue != true && y.MetadataInfo.HasValue)
        {
            return -1;
        }
        else if (x.MetadataInfo.HasValue == false && y.MetadataInfo.HasValue == false)
        {
            return 0;
        }

        string xId = x.MetadataInfo.Value.Id;
        string yId = y.MetadataInfo.Value.Id;

        if (x.DetailInfo.HasValue == false || y.DetailInfo.HasValue == false)
        {
            return string.CompareOrdinal(xId, yId);
        }

        if (xId == yId)
        {
            int xSort = x.DetailInfo.Value.StorySort;
            int ySort = y.DetailInfo.Value.StorySort;

            if (xSort == ySort)
            {
                return 0;
            }
            else
            {
                return Comparer<int>.Default.Compare(xSort, ySort);
            }
        }
        else
        {
            return string.CompareOrdinal(xId, yId);
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
        //null-coalescing操作符——当closeText为空时才赋值
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
