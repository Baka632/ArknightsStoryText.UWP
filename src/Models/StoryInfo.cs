using ArknightsStoryText.UWP.Helpers;

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
    /// 使用指定的参数构造<see cref="StoryInfo"/>的新实例
    /// </summary>
    /// <param name="title">剧情标题</param>
    /// <param name="text">剧情文本</param>
    public StoryInfo(string title, string text)
    {
        Title = title;
        Text = text;
    }

    public override string ToString()
    {
        string message = string.Format("Accessibility_StoryFileTitle".GetLocalized(), Title);
        return message;
    }
}
