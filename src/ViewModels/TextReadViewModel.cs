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

namespace ArknightsStoryText.UWP.ViewModels
{
    public sealed class TextReadViewModel : NotificationObject
    {
        private string _doctorName = string.Empty;
        private bool _isParagraph = false;
        private bool _isLoading = false;
        private readonly List<StorageFile> _originalFileList = new(20);

        public TextReadViewModel()
        {
            OpenStoryTextFileCommand = new DelegateCommand(async obj => await OpenStoryTextFileAsync());
            OpenStoryTextFolderCommand = new DelegateCommand(async obj => await OpenStoryTextFolderAsync());
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

        public ObservableCollection<StoryInfo> Stories { get; } = new();

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

        public List<double> FontSizes { get; } = new() { 8d, 9d, 10d, 11d, 12d, 14d, 16d, 18d, 20d, 24d, 28d, 36d, 48d, 72d };

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

            _originalFileList.AddRange(files);
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
            _originalFileList.AddRange(fileList);
            IsLoading = false;
        }

        private void ClearStoryTexts()
        {
            IsLoading = true;
            _originalFileList.Clear();
            Stories.Clear();
            IsLoading = false;
        }

        private void RemoveSingleStoryText(StoryInfo target)
        {
            _originalFileList.Remove(target.File);
            Stories.Remove(target);
        }

        private async Task<bool> ParseOriginTextFromStorageFileAsync(StorageFile file)
        {
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

                Stories.Add(new($"{file.DisplayName} [{"ParseFailed".GetLocalized()}]", $"{title}\n{message}", file));
                return false;
            }

            if (string.IsNullOrWhiteSpace(text))
            {
                string title = string.Format("FileIsEmpty_WithPlaceholder".GetLocalized(), file.Name);
                string message = "OpenAnotherFileInstead".GetLocalized();
                await ShowDialogAsync(title, message, closeText: "OK".GetLocalized());

                Stories.Add(new($"{file.DisplayName} [{"ParseFailed".GetLocalized()}]", $"{title}\n{message}",file));
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

                Stories.Add(new($"{file.DisplayName} [{"ParseFailed".GetLocalized()}]", $"{title}\n{message}", file));
                return false;
            }
            catch (Exception ex)
            {
                string title = string.Format("ErrorWhenParsing_WithPlaceholder".GetLocalized(), file.Name);
                string message = $"{ex.Message}\n{"OpenAnotherFileInstead".GetLocalized()}";
                await ShowDialogAsync(title, message, closeText: "OK".GetLocalized());

                Stories.Add(new($"{file.DisplayName} [{"ParseFailed".GetLocalized()}]", $"{title}\n{message}", file));
                return false;
            }

            string storyText = StoryReader.GetStoryText(scene.StoryCommands, IsParagraph);

            if (string.IsNullOrWhiteSpace(storyText))
            {
                Stories.Add(new($"{file.DisplayName} [{"ResultIsEmpty".GetLocalized()}]", storyText, file));
                return false;
            }

            Stories.Add(new(file.DisplayName, storyText, file));

            return true;
        }

        private async Task ReParseStoryTextAsync()
        {
            IsLoading = true;

            if (_originalFileList.Any())
            {
                Stories.Clear();

                foreach (StorageFile file in _originalFileList)
                {
                    await ParseOriginTextFromStorageFileAsync(file);
                }
            }

            IsLoading = false;
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
}
