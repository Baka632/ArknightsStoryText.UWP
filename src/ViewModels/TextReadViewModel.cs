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

namespace ArknightsStoryText.UWP.ViewModels
{
    internal class TextReadViewModel : NotificationObject
    {
        private string _originStoryText = "";
        private string _transformedStoryText = "";
        private string _doctorName = string.Empty;
        private bool _isParagraph = false;

        public TextReadViewModel()
        {
            OpenStoryTextFileCommand = new DelegateCommand(async (obj) => await OpenStoryTextFile());
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
                    _ = ParseOriginText(OriginStoryText);
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
                    _ = ParseOriginText(OriginStoryText);
                }
            }
        }

        private async Task OpenStoryTextFile()
        {
            FileOpenPicker fileOpenPicker = new();
            fileOpenPicker.FileTypeFilter.Add(".txt");
            StorageFile storageFile = await fileOpenPicker.PickSingleFileAsync();

            if (storageFile is null)
            {
                //用户取消了文件选择
                return;
            }

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

            await ParseOriginText(originText);
        }

        private async Task ParseOriginText(string originText)
        {
            try
            {
                StoryScene storyScene = await Task.Run(() =>
                {
                    StoryReader storyReader = new(originText, DoctorName);
                    StoryScene scene = storyReader.GetStoryScene();
                    return scene;
                });
                string transformedText = storyScene.GetStoryText(IsParagraph);
                TransformedStoryText = transformedText;

                if (string.IsNullOrWhiteSpace(transformedText))
                {
                    await ShowDialogAsync("ResultIsEmpty".GetLocalized(), "OpenAnotherFileInstead".GetLocalized());
                }
            }
            catch (ArgumentException)
            {
                await ShowDialogAsync("TutorialFileNotSupported".GetLocalized(), "OpenAnotherFileInstead".GetLocalized());
            }
        }

        private async static Task ShowDialogAsync(string title, string content)
        {
            ContentDialog dialog = new()
            {
                Title = title,
                Content = content,
                PrimaryButtonText = ReswHelper.GetReswString("Close")
            };

            await dialog.ShowAsync();
        }
    }
}
