namespace ArknightsStoryText.UWP.Models;

/// <summary>
/// 表示 <see cref="MainPage"/> 的 Pivot 中的页面
/// </summary>
public record PivotItemIdentifier(PivotItemType Index);

/// <summary>
/// <see cref="MainPage"/> 的 Pivot 内的页面种类
/// </summary>
public enum PivotItemType
{
    /// <summary>
    /// 剧情阅读页
    /// </summary>
    ReadPage = 0,
    /// <summary>
    /// 剧情合并页
    /// </summary>
    MergePage = 1,
    /// <summary>
    /// 剧情概览页
    /// </summary>
    GlancePage = 2,
}