using System.ComponentModel;

namespace ArknightsStoryText.UWP.Models;

/// <summary>
/// 记录原始剧情文件信息的类
/// </summary>
public record StoryFileInfo : INotifyPropertyChanged
{
    private string _title;
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
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Title)));
        }
    }

    /// <summary>
    /// 剧情元数据信息
    /// </summary>
    public StoryMetadataInfo? MetadataInfo { get; set; }
    /// <summary>
    /// 剧情的详细信息
    /// </summary>
    public InfoUnlockData? DetailInfo { get; set; }

    /// <summary>
    /// 使用指定的参数构造<seealso cref="StoryFileInfo"/>的新实例
    /// </summary>
    /// <param name="file">表示原始剧情文件的<seealso cref="StorageFile"/></param>
    /// <param name="title">原始剧情文件的标题</param>
    public StoryFileInfo(StorageFile file, string title, StoryMetadataInfo? metadataInfo, InfoUnlockData? detailInfo)
    {
        File = file;
        Title = title;
        MetadataInfo = metadataInfo;
        DetailInfo = detailInfo;
    }

    public override string ToString()
    {
        string message = string.Format("Accessibility_StoryFileInfo".GetLocalized(), File.DisplayName);
        return message;
    }
}
