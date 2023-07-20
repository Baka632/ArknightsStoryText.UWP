using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ArknightsStoryText.UWP.Commands;
using Windows.Storage.Pickers;
using Windows.Storage;
using System.Collections.ObjectModel;
using ArknightsStoryText.UWP.Models;
using ArknightsResources.Stories.Models;
using ArknightsResources.Utility;
using ArknightsStoryText.UWP.Helpers;
using Windows.UI.Xaml.Controls;
using System.Diagnostics;

namespace ArknightsStoryText.UWP.ViewModels
{
    public class TextMergeViewModel : NotificationObject
    {
        private string _transformedStoryText = "";
        private string _doctorName = string.Empty;
        private bool _isParagraph = false;
        private bool _isMerging = false;

        public TextMergeViewModel()
        {
            OpenStoryTextFileCommand = new DelegateCommand(async (obj) => await OpenStoryTextFileAsync());
            SaveStoryTextFileCommand = new DelegateCommand(async (obj) => await SaveStoryTextFileAsync());
            RemoveStoryTextFileCommand = new DelegateCommand(obj =>
            {
                if (obj is StoryFileInfo fileInfo)
                {
                    Files.Remove(fileInfo);
                }
            });
        }

        public ICommand OpenStoryTextFileCommand { get; }
        public ICommand SaveStoryTextFileCommand { get; }
        public ICommand RemoveStoryTextFileCommand { get; }

        public ObservableCollection<StoryFileInfo> Files { get; } = new();

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
            }
        }

        public bool IsParagraph
        {
            get => _isParagraph;
            set
            {
                _isParagraph = value;
                OnPropertiesChanged();
            }
        }

        public bool IsMerging
        {
            get => _isMerging;
            set
            {
                _isMerging = value;
                OnPropertiesChanged();
            }
        }

        private async Task OpenStoryTextFileAsync()
        {
            FileOpenPicker fileOpenPicker = new();
            fileOpenPicker.FileTypeFilter.Add(".txt");
            IReadOnlyList<StorageFile> storageFiles = await fileOpenPicker.PickMultipleFilesAsync();

            if (storageFiles is not null || !storageFiles.Any())
            {
                foreach (StorageFile item in storageFiles)
                {
                    if (!Files.Any(fileInfo => fileInfo.File.Path == item.Path))
                    {
                        Files.Add(new StoryFileInfo(item, string.Empty));
                    }
                }
            }
        }

        private async Task SaveStoryTextFileAsync()
        {
            StringBuilder stringBuilder = new(20);
            int count = 0;

            if (!Files.Any())
            {
                await ShowDialogAsync("NoFileImported".GetLocalized(), "AddSomeFiles".GetLocalized(), closeText:"Close".GetLocalized());
                return;
            }

            foreach (var item in Files)
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
                    //TODO: Localize

                    ContentDialogResult result = await ShowDialogAsync($"解析文件{item.File.Name}时出错",
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

            FileSavePicker fileSavePicker = new();
            fileSavePicker.FileTypeChoices.Add("TXT", new string[] { ".txt" });
            StorageFile saveFile = await fileSavePicker.PickSaveFileAsync();

            if (saveFile is not null)
            {
                await FileIO.WriteTextAsync(saveFile, stringBuilder.ToString());
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
}
