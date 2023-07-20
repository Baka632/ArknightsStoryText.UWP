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
using System.Text;

namespace ArknightsStoryText.UWP.ViewModels
{
    internal class TextReadViewModel : NotificationObject
    {
        private string _originStoryText = "";
        private string _transformedStoryText = "";
        private string _doctorName = string.Empty;
        private bool _isParagraph = false;
        private bool _isLoading = false;

        public TextReadViewModel()
        {
            OpenStoryTextFileCommand = new DelegateCommand(async (obj) => await OpenStoryTextFileAsync());
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
                //TODO: Localize
                await ShowDialogAsync($"解析文件时出错", $"{ex.Message}\n{ex.StackTrace}");
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
