namespace ArknightsStoryText.UWP.Helpers;

/// <summary>
/// 为按照剧情顺序排序而对 <see cref="StoryInfoComparer"/> 进行比较的类
/// </summary>
public sealed class StoryInfoComparer : IComparer<StoryInfo>
{
    /// <inheritdoc/>
    public int Compare(StoryInfo x, StoryInfo y)
    {
        if (x.MetadataInfo.HasValue && y.MetadataInfo.HasValue != true)
        {
            return 1;
        }
        else if (x.MetadataInfo.HasValue != true && y.MetadataInfo.HasValue)
        {
            return -1;
        }
        else if (x.MetadataInfo.HasValue == false && y.MetadataInfo.HasValue == false)
        {
            return 0;
        }

        string xId = x.MetadataInfo.Value.Id;
        string yId = y.MetadataInfo.Value.Id;

        if (x.DetailInfo.HasValue == false || y.DetailInfo.HasValue == false)
        {
            return string.CompareOrdinal(xId, yId);
        }

        if (xId == yId)
        {
            int xSort = x.DetailInfo.Value.StorySort;
            int ySort = y.DetailInfo.Value.StorySort;

            if (xSort == ySort)
            {
                return 0;
            }
            else
            {
                return Comparer<int>.Default.Compare(xSort, ySort);
            }
        }
        else
        {
            return string.CompareOrdinal(xId, yId);
        }
    }
}
