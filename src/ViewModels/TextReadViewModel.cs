using System;
using System.Threading.Tasks;
using ArknightsResources.Stories.Models;
using ArknightsResources.Utility;
using ArknightsStoryText.UWP.Helpers;
using Windows.Storage.Pickers;
using Windows.Storage;
using Windows.UI.Xaml.Controls;
using System.Windows.Input;
using ArknightsStoryText.UWP.Commands;
using System.Collections.Generic;
using Windows.UI.Xaml.Media;
using ArknightsStoryText.UWP.Models;
using Windows.Globalization.Fonts;
using System.Globalization;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.Json;
using System.IO;

namespace ArknightsStoryText.UWP.ViewModels
{
    public sealed class TextReadViewModel : NotificationObject
    {
        private string _doctorName = string.Empty;
        private bool _isParagraph = false;
        private bool _isLoading = false;
        private Dictionary<string, StoryMetadataInfo> _storyMetadataDict;
        private static readonly JsonSerializerOptions _defaultJsonOptions = new() { PropertyNameCaseInsensitive = true };
        private ObservableCollection<StoryInfo> stories = [];

        public TextReadViewModel()
        {
            OpenStoryTextFileCommand = new DelegateCommand(async obj => await OpenStoryTextFileAsync());
            OpenStoryTextFolderCommand = new DelegateCommand(async obj => await OpenStoryTextFolderAsync());
            LoadStoryMetadataCommand = new DelegateCommand(async obj => await OpenMetadataFileAsync());
            ClearStoryTextsCommand = new DelegateCommand(obj => ClearStoryTexts());
            RemoveSingleStoryTextCommand = new DelegateCommand(obj =>
            {
                if (obj is StoryInfo info)
                {
                    RemoveSingleStoryText(info);
                }
            });

            IReadOnlyList<FontInfo> fonts = FontHelper.GetSystemFonts();

            Fonts = fonts;
        }

        public ICommand OpenStoryTextFileCommand { get; }
        public ICommand OpenStoryTextFolderCommand { get; }
        public ICommand ClearStoryTextsCommand { get; }
        public ICommand RemoveSingleStoryTextCommand { get; }
        public ICommand LoadStoryMetadataCommand { get; }

        public ObservableCollection<StoryInfo> Stories
        {
            get => stories;
            set
            {
                stories = value;
                OnPropertiesChanged();
            }
        }

        public string DoctorName
        {
            get => _doctorName;
            set
            {
                if (_doctorName == value)
                {
                    return;
                }

                _doctorName = value;
                OnPropertiesChanged();

                _ = ReParseStoryTextAsync();
            }
        }

        public bool IsParagraph
        {
            get => _isParagraph;
            set
            {
                if (_isParagraph == value)
                {
                    return;
                }

                _isParagraph = value;
                OnPropertiesChanged();

                _ = ReParseStoryTextAsync();
            }
        }

        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                _isLoading = value;
                OnPropertiesChanged();
            }
        }

        public List<double> FontSizes { get; } = [8d, 9d, 10d, 11d, 12d, 14d, 16d, 18d, 20d, 24d, 28d, 36d, 48d, 72d];

        public IReadOnlyList<FontInfo> Fonts { get; }

        public static FontInfo DefaultFont
        {
            get
            {
                LanguageFontGroup languageFontGroup = new(CultureInfo.CurrentUICulture.Name);
                FontFamily defaultFont = new(languageFontGroup.ModernDocumentFont.FontFamily);
                return new(defaultFont.Source, defaultFont);
            }
        }

        public static double DefaultFontSize => 16;

        private async Task OpenStoryTextFileAsync()
        {
            FileOpenPicker fileOpenPicker = new();
            fileOpenPicker.FileTypeFilter.Add(".txt");
            IReadOnlyList<StorageFile> files = await fileOpenPicker.PickMultipleFilesAsync();

            if (files is null || files.Any() != true)
            {
                //用户取消了文件选择，或者文件列表为空
                return;
            }

            IsLoading = true;

            foreach (StorageFile file in files)
            {
                await ParseOriginTextFromStorageFileAsync(file);
            }
            SortStoryList();

            IsLoading = false;
        }

        private async Task OpenStoryTextFolderAsync()
        {
            FolderPicker folderPicker = new();
            StorageFolder folder = await folderPicker.PickSingleFolderAsync();

            if (folder is null)
            {
                //用户取消了文件夹选择
                return;
            }

            IsLoading = true;

            Stories.Clear();
            IEnumerable<StorageFile> fileList = (await folder.GetFilesAsync()).Where(file => file.Name.EndsWith(".txt", StringComparison.OrdinalIgnoreCase));
            foreach (StorageFile file in fileList)
            {
                await ParseOriginTextFromStorageFileAsync(file);
            }
            SortStoryList();

            IsLoading = false;
        }

        private async Task OpenMetadataFileAsync()
        {
            FileOpenPicker fileOpenPicker = new();
            fileOpenPicker.FileTypeFilter.Add(".json");
            StorageFile file = await fileOpenPicker.PickSingleFileAsync();

            if (file is null)
            {
                //用户取消了文件选择
                return;
            }

            IsLoading = true;

            Stream utf8Json = await file.OpenStreamForReadAsync();

            try
            {
                Dictionary<string, StoryMetadataInfo> metadataDict = await JsonSerializer.DeserializeAsync<Dictionary<string, StoryMetadataInfo>>(utf8Json, _defaultJsonOptions);

                if (metadataDict.ContainsKey("1stact"))
                {
                    _storyMetadataDict = metadataDict;
                }
                else
                {
                    await ShowWrongMetadataDialog();
                }
            }
            catch (JsonException)
            {
                await ShowWrongMetadataDialog();
            }

            await ReParseStoryTextAsync();

            IsLoading = false;

            static async Task ShowWrongMetadataDialog()
            {
                //TODO: Localize
                await ShowDialogAsync("Error!", "Wrong metadata file");
            }
        }

        private void ClearStoryTexts()
        {
            IsLoading = true;
            Stories.Clear();
            IsLoading = false;
        }

        private void RemoveSingleStoryText(StoryInfo target)
        {
            Stories.Remove(target);
        }

        private async Task<bool> ParseOriginTextFromStorageFileAsync(StorageFile file)
        {
            string storyDisplayName;
            StoryMetadataInfo? metadata;
            InfoUnlockData? detailInfo;

            if (TryGetMetadataFromMetadataDict(file.DisplayName, out (StoryMetadataInfo, InfoUnlockData) result))
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

                Stories.Add(new($"{storyDisplayName} [{"ParseFailed".GetLocalized()}]", $"{title}\n{message}", file, metadata, detailInfo));
                return false;
            }

            if (string.IsNullOrWhiteSpace(text))
            {
                string title = string.Format("FileIsEmpty_WithPlaceholder".GetLocalized(), file.Name);
                string message = "OpenAnotherFileInstead".GetLocalized();
                await ShowDialogAsync(title, message, closeText: "OK".GetLocalized());

                Stories.Add(new($"{storyDisplayName} [{"ParseFailed".GetLocalized()}]", $"{title}\n{message}", file, metadata, detailInfo));
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

                Stories.Add(new($"{storyDisplayName} [{"ParseFailed".GetLocalized()}]", $"{title}\n{message}", file, metadata, detailInfo));
                return false;
            }
            catch (Exception ex)
            {
                string title = string.Format("ErrorWhenParsing_WithPlaceholder".GetLocalized(), file.Name);
                string message = $"{ex.Message}\n{"OpenAnotherFileInstead".GetLocalized()}";
                await ShowDialogAsync(title, message, closeText: "OK".GetLocalized());

                Stories.Add(new($"{storyDisplayName} [{"ParseFailed".GetLocalized()}]", $"{title}\n{message}", file, metadata, detailInfo));
                return false;
            }

            string storyText = StoryReader.GetStoryText(scene.StoryCommands, IsParagraph);

            if (string.IsNullOrWhiteSpace(storyText))
            {
                Stories.Add(new($"{storyDisplayName} [{"ResultIsEmpty".GetLocalized()}]", storyText, file, metadata, detailInfo));
                return false;
            }

            Stories.Add(new(storyDisplayName, storyText, file, metadata, detailInfo));

            return true;
        }

        private async Task ReParseStoryTextAsync()
        {
            IsLoading = true;

            if (Stories.Count > 0)
            {
                StorageFile[] _originalFileList = (from info in Stories select info.File).ToArray();
                Stories.Clear();

                foreach (StorageFile file in _originalFileList)
                {
                    await ParseOriginTextFromStorageFileAsync(file);
                }

                SortStoryList();
            }

            IsLoading = false;
        }

        private bool TryGetMetadataFromMetadataDict(string storyFileName, out (StoryMetadataInfo, InfoUnlockData) result)
        {
            if (_storyMetadataDict is not null && string.IsNullOrWhiteSpace(storyFileName) != true)
            {
                foreach (StoryMetadataInfo info in _storyMetadataDict.Values)
                {
                    foreach (InfoUnlockData data in info.InfoUnlockDatas)
                    {
                        if (data.StoryTxt.AsSpan().EndsWith(storyFileName.AsSpan()))
                        {
                            result = (info, data);
                            return true;
                        }
                    }
                }
            }

            result = default;
            return false;
        }

        private void SortStoryList()
        {
            //TODO: 优化一下
            if (_storyMetadataDict is not null)
            {
                List<StoryInfo> storyList = [..Stories];
                storyList.Sort(new StoryInfoComparer());

                Stories = new(storyList);
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

    file sealed class StoryInfoComparer : IComparer<StoryInfo>
    {
        /// <inheritdoc/>
        public int Compare(StoryInfo x, StoryInfo y)
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
    }
}
