namespace ArknightsStoryText.UWP.Models;

/// <summary>
/// 表示剧情信息的类
/// </summary>
public sealed record StoryInfo
{
    /// <summary>
    /// 获取剧情标题
    /// </summary>
    public string Title { get; init; }
    /// <summary>
    /// 获取剧情文本
    /// </summary>
    public string Text { get; init; }
    /// <summary>
    /// 获取剧情简介
    /// </summary>
    public string Description { get; init; }
    /// <summary>
    /// 剧情的物理文件
    /// </summary>
    public StorageFile File { get; init; }
    /// <summary>
    /// 剧情元数据信息
    /// </summary>
    public StoryMetadataInfo? MetadataInfo { get; init; }
    /// <summary>
    /// 剧情的详细信息
    /// </summary>
    public InfoUnlockData? DetailInfo { get; init; }

    /// <summary>
    /// 使用指定的参数构造<see cref="StoryInfo"/>的新实例
    /// </summary>
    /// <param name="title">剧情标题</param>
    /// <param name="text">剧情文本</param>
    /// <param name="description">剧情简介</param>
    /// <param name="file">剧情的物理文件</param>
    /// <param name="metadataInfo">剧情的元数据信息</param>
    /// <param name="detailInfo">剧情的详细信息</param>
    public StoryInfo(string title, string text, string description, StorageFile file, StoryMetadataInfo? metadataInfo, InfoUnlockData? detailInfo)
    {
        Title = title;
        Text = text;
        Description = description;
        File = file;
        MetadataInfo = metadataInfo;
        DetailInfo = detailInfo;
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        string message = string.Format("Accessibility_StoryFileTitle".GetLocalized(), Title);
        return message;
    }
}
