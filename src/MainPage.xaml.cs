using ArknightsResources.Stories.Models;
using ArknightsResources.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Foundation.Metadata;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.System.Profile;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace ArknightsStoryText.UWP
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page, INotifyPropertyChanged
    {
        public MainPage()
        {
            this.InitializeComponent();

            if (ApiInformation.IsTypePresent(typeof(AcrylicBrush).FullName))
            {
                if (Application.Current.Resources["SystemControlAcrylicWindowBrush"] is AcrylicBrush brush)
                {
                    this.Background = brush;
                }
            }

            if (AnalyticsInfo.VersionInfo.DeviceFamily == "Windows.Mobile")
            {
                TitleBarTextBlock.Visibility = Visibility.Collapsed;
            }
        }

        private string _originStoryText = "";
        private string _transformedStoryText = "";
        private string _doctorName = string.Empty;
        private bool _isParagraph = false;

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

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 通知系统属性已经发生更改
        /// </summary>
        /// <param name="propertyName">发生更改的属性名称,其填充是自动完成的</param>
        public void OnPropertiesChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private async void OpenStoryTextFile(object sender, RoutedEventArgs e)
        {
            FileOpenPicker fileOpenPicker = new FileOpenPicker();
            fileOpenPicker.FileTypeFilter.Add(".txt");
            StorageFile storageFile = await fileOpenPicker.PickSingleFileAsync();

            if (storageFile is null)
            {
                return;
            }

            string originText = await FileIO.ReadTextAsync(storageFile);
            OriginStoryText = originText;
            StoryScene storyScene = await Task.Run(() =>
            {
                StoryReader storyReader = new StoryReader(originText, DoctorName);
                StoryScene scene = storyReader.GetStoryScene();
                return scene;
            });

            string transformedText = storyScene.GetStoryText(IsParagraph);
            TransformedStoryText = transformedText;
        }
    }
}
