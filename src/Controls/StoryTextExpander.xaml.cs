﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using ArknightsStoryText.UWP.Helpers;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

//https://go.microsoft.com/fwlink/?LinkId=234236 上介绍了“用户控件”项模板

namespace ArknightsStoryText.UWP.Controls
{
    public sealed partial class StoryTextExpander : UserControl
    {
        public StoryTextExpander()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// 获取或设置剧情文本的标题
        /// </summary>
        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        /// <summary>
        /// 用以标识 <see cref="StoryTextTitle"/> 的依赖属性
        /// </summary>
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(StoryTextExpander), new PropertyMetadata(null));

        /// <summary>
        /// 获取或设置剧情文本
        /// </summary>
        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        /// <summary>
        /// 用以标识 <see cref="StoryText"/> 的依赖属性
        /// </summary>
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(StoryTextExpander), new PropertyMetadata(null));

        /// <summary>
        /// 获取或设置控件是否展开的值
        /// </summary>
        public bool IsExpanded
        {
            get => (bool)GetValue(IsExpandedProperty);
            set
            {
                SetValue(IsExpandedProperty, value);
                OnIsExpandedChanged(value);
            }
        }

        private void OnIsExpandedChanged(bool value)
        {
            if (value)
            {
                //状态：展开
                ChevronIcon = "\uE70E";
                StoryTextVisibility = Visibility.Visible;
                StoryTextViewableToggleButtonAutomationName = "ClickToCollapse".GetLocalized();
            }
            else
            {
                //状态：折叠
                ChevronIcon = "\uE70D";
                StoryTextVisibility = Visibility.Collapsed;
                StoryTextViewableToggleButtonAutomationName = "ClickToExpand".GetLocalized();
            }
        }

        /// <summary>
        /// 用以标识 <see cref="IsExpanded"/> 的依赖属性
        /// </summary>
        public static readonly DependencyProperty IsExpandedProperty =
            DependencyProperty.Register("IsExpanded", typeof(bool), typeof(StoryTextExpander), new PropertyMetadata(true));

        private string ChevronIcon
        {
            get => (string)GetValue(ChevronIconProperty);
            set => SetValue(ChevronIconProperty, value);
        }

        private static readonly DependencyProperty ChevronIconProperty =
            DependencyProperty.Register("ChevronIcon", typeof(string), typeof(StoryTextExpander), new PropertyMetadata("\uE70E"));

        private Visibility StoryTextVisibility
        {
            get => (Visibility)GetValue(StoryTextVisibilityProperty);
            set => SetValue(StoryTextVisibilityProperty, value);
        }

        private static readonly DependencyProperty StoryTextVisibilityProperty =
            DependencyProperty.Register("StoryTextVisibility", typeof(Visibility), typeof(StoryTextExpander), new PropertyMetadata(Visibility.Visible));

        private string StoryTextViewableToggleButtonAutomationName
        {
            get => (string)GetValue(StoryTextViewableToggleButtonAutomationNameProperty);
            set => SetValue(StoryTextViewableToggleButtonAutomationNameProperty, value);
        }

        // Using a DependencyProperty as the backing store for StoryTextViewableToggleButtonAutomationName.  This enables animation, styling, binding, etc...
        private static readonly DependencyProperty StoryTextViewableToggleButtonAutomationNameProperty =
            DependencyProperty.Register("StoryTextViewableToggleButtonAutomationName", typeof(string), typeof(StoryTextExpander), new PropertyMetadata("ClickToCollapse".GetLocalized()));

        private void OnTitleGridTapped(object sender, TappedRoutedEventArgs e)
        {
            IsExpanded = !IsExpanded;
        }

        private void OnStoryTextViewableToggleButtonClicked(object sender, RoutedEventArgs e)
        {
            ToggleButton toggleButton = (ToggleButton)sender;
            IsExpanded = !toggleButton.IsChecked.Value;
        }
    }
}
