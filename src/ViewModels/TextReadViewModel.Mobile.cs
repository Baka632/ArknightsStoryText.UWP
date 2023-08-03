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
using Windows.UI.Xaml.Documents;
using Windows.Globalization.Fonts;
using System.Globalization;

namespace ArknightsStoryText.UWP.ViewModels
{
    internal class TextReadViewModel_Mobile : NotificationObject
    {
        private string _originStoryText = "";
        private string _transformedStoryText = "";
        private string _doctorName = string.Empty;
        private bool _isParagraph = false;
        private bool _isLoading = false;

        public TextReadViewModel_Mobile()
        {
            OpenStoryTextFileCommand = new DelegateCommand(async (obj) => await OpenStoryTextFileAsync());

            IReadOnlyList<FontInfo> fonts = FontHelper.GetSystemFonts();

            Fonts = fonts;
        }

        public ICommand OpenStoryTextFileCommand { get; }

        public string OriginStoryText
        {
            get => _originStoryText;
            set
            {
                _originStoryText = value;
                OnPropertiesChanged();
            }
        }

        public string TransformedStoryText
        {
            get => _transformedStoryText;
            set
            {
                _transformedStoryText = value;
                OnPropertiesChanged();
            }
        }

        public string DoctorName
        {
            get => _doctorName;
            set
            {
                _doctorName = value;
                OnPropertiesChanged();

                if (!string.IsNullOrWhiteSpace(OriginStoryText))
                {
                    _ = ParseOriginTextAsync(OriginStoryText);
                }
            }
        }

        public bool IsParagraph
        {
            get => _isParagraph;
            set
            {
                _isParagraph = value;
                OnPropertiesChanged();

                if (!string.IsNullOrWhiteSpace(OriginStoryText))
                {
                    _ = ParseOriginTextAsync(OriginStoryText);
                }
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

        public List<double> FontSizes { get; } = new() { 8, 9, 10, 11, 12, 14, 16, 18, 20, 24, 28, 36, 48, 72 };

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
            StorageFile storageFile = await fileOpenPicker.PickSingleFileAsync();

            if (storageFile is null)
            {
                //用户取消了文件选择
                return;
            }

            IsLoading = true;

            string originText;
            try
            {
                originText = await FileIO.ReadTextAsync(storageFile);
                OriginStoryText = originText;
            }
            catch (ArgumentOutOfRangeException)
            {
                await ShowDialogAsync("InvaildFile".GetLocalized(), "OpenAnotherFileInstead".GetLocalized());
                return;
            }

            if (string.IsNullOrWhiteSpace(originText))
            {
                await ShowDialogAsync("FileIsEmpty".GetLocalized(), "OpenAnotherFileInstead".GetLocalized());
                return;
            }

            await ParseOriginTextAsync(originText);

            IsLoading = false;
        }

        private async Task ParseOriginTextAsync(string originText)
        {
            if (IsLoading is not true)
            {
                IsLoading = true;
            }
            TransformedStoryText = string.Empty;

            StoryReader storyReader = new(originText, DoctorName);
            StoryScene storyScene;
            try
            {
                storyScene = storyReader.GetStoryScene();
            }
            catch (ArgumentException)
            {
                await ShowDialogAsync("TutorialFileNotSupported".GetLocalized(), "OpenAnotherFileInstead".GetLocalized());
                return;
            }
            catch (Exception ex)
            {
                await ShowDialogAsync("ErrorWhenParsing".GetLocalized(), $"{ex.Message}\n{ex.StackTrace}");
                return;
            }

            string transformedText = StoryReader.GetStoryText(storyScene.StoryCommands, IsParagraph);
            TransformedStoryText = transformedText;

            if (string.IsNullOrWhiteSpace(transformedText))
            {
                await ShowDialogAsync("ResultIsEmpty".GetLocalized(), "OpenAnotherFileInstead".GetLocalized());
            }

            IsLoading = false;
        }

        private async static Task ShowDialogAsync(string title, string content)
        {
            ContentDialog dialog = new()
            {
                Title = title,
                Content = content,
                CloseButtonText = ReswHelper.GetReswString("Close")
            };

            await dialog.ShowAsync();
        }
    }
}
