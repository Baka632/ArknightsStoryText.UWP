using System.IO;
using ArknightsStoryText.UWP.Services;
using Windows.Storage.Pickers;

namespace ArknightsStoryText.UWP.ViewModels;

public sealed partial class StoryGlanceViewModel : ObservableObject
{
    private readonly StoryMetadataService metadataService = new();
    private StorageFolder storyTextFolder = null;

    [ObservableProperty]
    private bool _isLoading = false;
    [ObservableProperty]
    private string fileNameLoading;
    [ObservableProperty]
    private IEnumerable<StoryGroup> storyGroups;

    [RelayCommand]
    private async Task OpenGameDataFolder()
    {
        FolderPicker folderPicker = new();
        StorageFolder gamedataFolder = await folderPicker.PickSingleFolderAsync();

        if (gamedataFolder is null)
        {
            //用户取消了文件夹选择
            return;
        }

        IsLoading = true;

        try
        {
            #region 加载剧情元数据
            StorageFolder excelFolder = await gamedataFolder.GetFolderAsync("excel");
            StorageFile storyMetadataFile = await excelFolder.GetFileAsync("story_review_table.json");
            FileNameLoading = storyMetadataFile.Name;

            Stream utf8Json = await storyMetadataFile.OpenStreamForReadAsync();
            if (!metadataService.TryInitialize(utf8Json))
            {
                string title = string.Format("InvaildFileInGameDataFolder_WithPlaceholder".GetLocalized(), storyMetadataFile.Name);
                string message = "OpenAnotherFolderInstead".GetLocalized();
                await ShowDialogAsync(title, message, closeText: "OK".GetLocalized());
                return;
            }
            #endregion

            StorageFolder storyFolder = await gamedataFolder.GetFolderAsync("story");
            storyTextFolder = storyFolder;
            List<StoryDataPack> storyGroups = new(1000);

            #region 加载剧情简介
            StorageFolder storyInfoFolder = await storyFolder.GetFolderAsync("[uc]info");

            List<StoryDataPack> storyDataPacks = new(metadataService.StoryMetadata.Values.Count);
            foreach (StoryMetadataInfo info in metadataService.StoryMetadata.Values)
            {
                List<StoryInfo> storyInfos = null;
                foreach (InfoUnlockData data in info.InfoUnlockDatas)
                {
                    List<string> strParts = new(3);

                    if (string.IsNullOrWhiteSpace(data.StoryCode) != true)
                    {
                        strParts.Add(data.StoryCode);
                    }

                    strParts.Add(data.StoryName);

                    if (data.AvgTag != "幕间")
                    {
                        strParts.Add(data.AvgTag);
                    }

                    string storyDisplayName = string.Join(' ', strParts);
                    string storyDescription = string.Empty;

                    string path = $"{data.StoryInfo.Replace('/', Path.DirectorySeparatorChar).Replace($"info{Path.DirectorySeparatorChar}", $"[uc]info{Path.DirectorySeparatorChar}")}.txt";

                    StorageFile storyDescFile = await GetStorageFileByPath(path, storyInfoFolder);

                    if (storyDescFile != null)
                    {
                        FileNameLoading = storyDescFile.Name;
                        storyDescription = await FileIO.ReadTextAsync(storyDescFile);
                    }

                    StoryInfo storyInfo = new(storyDisplayName, string.Empty, storyDescription, null, info, data);
                    if (storyInfos is null)
                    {
                        storyInfos = new List<StoryInfo>(info.InfoUnlockDatas.Length) { storyInfo };
                    }
                    else
                    {
                        storyInfos.Add(storyInfo);
                    }
                }
                StoryDataPack storyDataPack = new(info.Name, storyInfos, info);
                storyDataPacks.Add(storyDataPack);
            }

            IEnumerable<StoryGroup> group = storyDataPacks.GroupBy(pack => pack.StoryMetadata.ActType,
                (actType, pack) => new StoryGroup(actType, pack));
            StoryGroups = group;
            #endregion
        }
        catch (FileNotFoundException)
        {
            await ShowDialogAsync("RequiredFileOrFolderNotFoundInGameDataFolder".GetLocalized(),
                                  "OpenAnotherFolderInstead".GetLocalized(),
                                  closeText: "OK".GetLocalized());
        }
        finally
        {
            IsLoading = false;
        }
    }

    private static async Task<StorageFile> GetStorageFileByPath(string path, StorageFolder baseFolder)
    {
        string[] pathParts = path.Split(Path.DirectorySeparatorChar);
        StorageFolder currentFolder = baseFolder;

        foreach (string part in pathParts)
        {
            if (Path.HasExtension(part) || pathParts[^1] == part)
            {
                StorageFile file = await currentFolder.GetFileAsync(part);
                return file;
            }
            else
            {
                if (part.Equals(currentFolder.DisplayName, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }
                else
                {
                    currentFolder = await currentFolder.GetFolderAsync(part);
                }
            }
        }

        return null;
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
