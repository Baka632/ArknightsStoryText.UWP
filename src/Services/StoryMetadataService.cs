using System.IO;
using System.Collections.Immutable;

namespace ArknightsStoryText.UWP.Services;

/// <summary>
/// 剧情元数据服务
/// </summary>
public sealed class StoryMetadataService
{
    private ImmutableDictionary<string, StoryMetadataInfo> _storyMetadataDict;
    private static readonly JsonSerializerOptions _defaultJsonOptions = new() { PropertyNameCaseInsensitive = true };

    /// <summary>
    /// 获取 <see cref="StoryMetadataService"/> 的默认实例
    /// </summary>
    public static StoryMetadataService Default { get; } = new StoryMetadataService();
    /// <summary>
    /// 确定此实例是否已经初始化的值
    /// </summary>
    public bool IsInitialized { get => _storyMetadataDict != null; }
    /// <summary>
    /// 包含剧情元数据的不可变字典。如果尚未初始化当前的 <see cref="StoryMetadataService"/> 实例，则此属性为 <see langword="null"/>。
    /// </summary>
    public ImmutableDictionary<string, StoryMetadataInfo> StoryMetadata { get; }

    /// <summary>
    /// 尝试将此实例初始化
    /// </summary>
    /// <param name="utf8Json">包含以 UTF-8 编码的剧情元数据文件的 JSON 流</param>
    /// <returns>指示初始化过程是否成功的值</returns>
    public bool TryInitialize(Stream utf8Json)
    {
        try
        {
            ImmutableDictionary<string, StoryMetadataInfo> metadataDict = JsonSerializer.Deserialize<ImmutableDictionary<string, StoryMetadataInfo>>(utf8Json, _defaultJsonOptions);

            if (metadataDict.ContainsKey("1stact"))
            {
                _storyMetadataDict = metadataDict;
                return true;
            }
            else
            {
                return false;
            }
        }
        catch (JsonException)
        {
            return false;
        }
    }

    /// <summary>
    /// 尝试从剧情元数据字典中获取需要的元数据
    /// </summary>
    /// <param name="storyFileName">不带扩展名的剧情文件名</param>
    /// <param name="result">包含指定剧情文件元数据的元组</param>
    /// <returns>指示过程是否成功的值</returns>
    public bool TryGetMetadata(string storyFileName, out (StoryMetadataInfo, InfoUnlockData) result)
    {
        if (_storyMetadataDict is not null && string.IsNullOrWhiteSpace(storyFileName) != true)
        {
            foreach (StoryMetadataInfo info in _storyMetadataDict.Values)
            {
                foreach (InfoUnlockData data in info.InfoUnlockDatas)
                {
                    if (data.StoryTxt.AsSpan().EndsWith(storyFileName.AsSpan()))
                    {
                        result = (info, data);
                        return true;
                    }
                }
            }
        }

        result = default;
        return false;
    }
}
