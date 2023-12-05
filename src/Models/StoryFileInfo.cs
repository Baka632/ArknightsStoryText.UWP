namespace ArknightsStoryText.UWP.Models;

/// <summary>
/// 记录原始剧情文件信息的类
/// </summary>
public sealed class StoryFileInfo : NotificationObject
{
    /// <summary>
    /// 获取表示原始剧情文件的<seealso cref="StorageFile"/>
    /// </summary>
    public StorageFile File { get; }

    /// <summary>
    /// 获取或设置原始剧情文件的标题
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// 使用指定的参数构造<seealso cref="StoryFileInfo"/>的新实例
    /// </summary>
    /// <param name="file">表示原始剧情文件的<seealso cref="StorageFile"/></param>
    /// <param name="title">原始剧情文件的标题</param>
    /// <exception cref="ArgumentNullException">当参数为 Null 时抛出</exception>
    public StoryFileInfo(StorageFile file, string title = "")
    {
        File = file ?? throw new ArgumentNullException(nameof(file));
        Title = title ?? throw new ArgumentNullException(nameof(title));
    }

    public override string ToString()
    {
        string message = string.Format("Accessibility_StoryFileInfo".GetLocalized(), File.DisplayName);
        return message;
    }
}
