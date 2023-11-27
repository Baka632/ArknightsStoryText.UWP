namespace ArknightsStoryText.UWP.Models;

/// <summary>
/// 表示剧情元数据的结构
/// </summary>
public struct StoryMetadataInfo
{
    /// <summary>
    /// 剧情的 ID
    /// </summary>
    public string Id { get; set; }
    /// <summary>
    /// 剧情名称
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// 活动类型
    /// </summary>
    public string EntryType { get; set; }
    /// <summary>
    /// 剧情类型
    /// </summary>
    public string ActType { get; set; }
    /// <summary>
    /// 活动开始时间的 Unix 时间戳
    /// </summary>
    public long StartTime { get; set; }
    /// <summary>
    /// 活动结束时间的 Unix 时间戳
    /// </summary>
    public long EndTime { get; set; }
    /// <summary>
    /// 活动何时开始在终端展示的 Unix 时间戳
    /// </summary>
    public long StartShowTime { get; set; }
    /// <summary>
    /// 活动何时停止在终端展示的 Unix 时间戳
    /// </summary>
    public long EndShowTime { get; set; }
    /// <summary>
    /// 活动复刻开始时间的 Unix 时间戳
    /// </summary>
    public long RemakeStartTime { get; set; }
    /// <summary>
    /// 活动复刻结束时间的 Unix 时间戳
    /// </summary>
    public long RemakeEndTime { get; set; }
    public string StoryEntryPicId { get; set; }
    public string StoryPicId { get; set; }
    public string StoryMainColor { get; set; }
    public int CustomType { get; set; }
    public string StoryCompleteMedalId { get; set; }
    /// <summary>
    /// 剧情解锁后获得奖励的信息
    /// </summary>
    public RewardsInfo[] Rewards { get; set; }
    /// <summary>
    /// 剧情解锁后的详细信息
    /// </summary>
    public InfoUnlockData[] InfoUnlockDatas { get; set; }
}

/// <summary>
/// 表示剧情解锁后信息的结构
/// </summary>
public struct InfoUnlockData
{
    public string StoryReviewType { get; set; }
    /// <summary>
    /// 剧情文本的 ID
    /// </summary>
    public string StoryId { get; set; }
    /// <summary>
    /// 剧情所属的组（剧情 ID）
    /// </summary>
    public string StoryGroup { get; set; }
    public int StorySort { get; set; }
    /// <summary>
    /// 剧情依赖项（上一个剧情）的 ID
    /// </summary>
    public string StoryDependence { get; set; }
    public int StoryCanShow { get; set; }
    /// <summary>
    /// 剧情活动代号（如 NL-ST-2）
    /// </summary>
    public string StoryCode { get; set; }
    /// <summary>
    /// 剧情名称
    /// </summary>
    public string StoryName { get; set; }
    public string StoryPic { get; set; }
    /// <summary>
    /// 指向剧情简介的相对路径
    /// </summary>
    public string StoryInfo { get; set; }
    public int StoryCanEnter { get; set; }
    /// <summary>
    /// 指向剧情文本的相对路径
    /// </summary>
    public string StoryTxt { get; set; }
    /// <summary>
    /// 剧情的标签（如“幕间”，“行动前”等）
    /// </summary>
    public string AvgTag { get; set; }
    public string UnLockType { get; set; }
    public string CostItemType { get; set; }
    public string CostItemId { get; set; }
    public int CostItemCount { get; set; }
    public int StageCount { get; set; }
    /// <summary>
    /// 剧情解锁所需要的前置关卡的信息
    /// </summary>
    public RequiredStageInfo[] RequiredStages { get; set; }
}

/// <summary>
/// 表示剧情前置关卡信息的结构
/// </summary>
public struct RequiredStageInfo
{
    /// <summary>
    /// 前置关卡的 ID
    /// </summary>
    public string StageId { get; set; }
    /// <summary>
    /// 前置关卡状态的最低要求
    /// </summary>
    public string MinState { get; set; }
    /// <summary>
    /// 前置关卡状态的最高要求
    /// </summary>
    public string MaxState { get; set; }
}

/// <summary>
/// 表示剧情解锁奖励信息的结构
/// </summary>
public struct RewardsInfo
{
    /// <summary>
    /// 奖励物品的 ID
    /// </summary>
    public string Id { get; set; }
    /// <summary>
    /// 奖励物品的数量
    /// </summary>
    public int Count { get; set; }
    /// <summary>
    /// 奖励物品的类型
    /// </summary>
    public string Type { get; set; }
}
