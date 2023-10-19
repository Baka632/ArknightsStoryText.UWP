using ArknightsStoryText.UWP.Helpers;
using Windows.Storage;

namespace ArknightsStoryText.UWP.Models;

/// <summary>
/// 表示剧情信息的类
/// </summary>
public sealed class StoryInfo
{
    /// <summary>
    /// 获取剧情标题
    /// </summary>
    public string Title { get; }
    /// <summary>
    /// 获取剧情文本
    /// </summary>
    public string Text { get; }
    /// <summary>
    /// 剧情的物理文件
    /// </summary>
    public StorageFile File { get; }

    /// <summary>
    /// 使用指定的参数构造<see cref="StoryInfo"/>的新实例
    /// </summary>
    /// <param name="title">剧情标题</param>
    /// <param name="text">剧情文本</param>
    /// <param name="file">剧情的物理文件</param>
    public StoryInfo(string title, string text, StorageFile file)
    {
        Title = title;
        Text = text;
        File = file;
    }

    public override string ToString()
    {
        string message = string.Format("Accessibility_StoryFileTitle".GetLocalized(), Title);
        return message;
    }
}
