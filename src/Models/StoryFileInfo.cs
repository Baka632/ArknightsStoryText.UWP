using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ArknightsStoryText.UWP.Models;

/// <summary>
/// 记录原始剧情文件信息的类
/// </summary>
public record StoryFileInfo : INotifyPropertyChanged
{
    private string _title;
    private InfoUnlockData? _detailInfo;
    private string _description;

    public event PropertyChangedEventHandler PropertyChanged;

    /// <summary>
    /// 获取表示原始剧情文件的<seealso cref="StorageFile"/>
    /// </summary>
    public StorageFile File { get; }

    /// <summary>
    /// 获取或设置原始剧情文件的标题
    /// </summary>
    public string Title
    {
        get => _title;
        set
        {
            _title = value;
            OnPropertiesChanged();
        }
    }

    /// <summary>
    /// 获取剧情简介
    /// </summary>
    public string Description
    {
        get => _description;
        set
        {
            _description = value;
            OnPropertiesChanged();
        }
    }

    /// <summary>
    /// 剧情的显示名称
    /// </summary>
    public string DisplayTitle
    {
        get => DetailInfo.HasValue
                ? GetStoryDisplayName(DetailInfo.Value)
                : File.DisplayName;
    }

    /// <summary>
    /// 剧情元数据信息
    /// </summary>
    public StoryMetadataInfo? MetadataInfo { get; set; }
    /// <summary>
    /// 剧情的详细信息
    /// </summary>
    public InfoUnlockData? DetailInfo
    {
        get => _detailInfo;
        set
        {
            _detailInfo = value;
            OnPropertiesChanged();
            OnPropertiesChanged(nameof(DisplayTitle));
        }
    }

    private static string GetStoryDisplayName(InfoUnlockData info)
    {
        string storyDisplayName;
        List<string> strParts = new(3);

        if (string.IsNullOrWhiteSpace(info.StoryCode) != true)
        {
            strParts.Add(info.StoryCode);
        }

        strParts.Add(info.StoryName);

        if (info.AvgTag != "幕间")
        {
            strParts.Add(info.AvgTag);
        }

        storyDisplayName = string.Join(' ', strParts);
        return storyDisplayName;
    }

    /// <summary>
    /// 使用指定的参数构造 <seealso cref="StoryFileInfo"/> 的新实例
    /// </summary>
    /// <param name="file">表示原始剧情文件的 <seealso cref="StorageFile"/></param>
    /// <param name="title">原始剧情文件的标题</param>
    /// <param name="description">剧情简介</param>
    /// <param name="metadataInfo">剧情元数据信息</param>
    /// <param name="detailInfo">剧情详细信息</param>
    public StoryFileInfo(StorageFile file, string title, string description, StoryMetadataInfo? metadataInfo, InfoUnlockData? detailInfo)
    {
        File = file;
        Title = title;
        Description = description;
        MetadataInfo = metadataInfo;
        DetailInfo = detailInfo;
    }

    public override string ToString()
    {
        string message = string.Format("Accessibility_StoryFileInfo".GetLocalized(), File.DisplayName);
        return message;
    }

    /// <summary>
    /// 通知运行时属性已经发生更改
    /// </summary>
    /// <param name="propertyName">发生更改的属性名称,其填充是自动完成的</param>
    public void OnPropertiesChanged([CallerMemberName] string propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
