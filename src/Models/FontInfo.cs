namespace ArknightsStoryText.UWP.Models;

/// <summary>
/// 表示一个字体信息的类
/// </summary>
public sealed class FontInfo : NotificationObject, IEquatable<FontInfo>
{
    /// <summary>
    /// 显示用字体名称
    /// </summary>
    public string DisplayFontName { get; }
    /// <summary>
    /// 表示字体的<seealso cref="Windows.UI.Xaml.Media.FontFamily"/>
    /// </summary>
    public FontFamily FontFamily { get; }

    /// <summary>
    /// 使用指定的参数构造<seealso cref="FontInfo"/>的新实例
    /// </summary>
    /// <param name="displayFontName">显示用字体名称</param>
    /// <param name="fontFamily">表示字体的<seealso cref="Windows.UI.Xaml.Media.FontFamily"/></param>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    public FontInfo(string displayFontName, FontFamily fontFamily)
    {
        if (string.IsNullOrWhiteSpace(displayFontName))
        {
            throw new ArgumentException($"“{nameof(displayFontName)}”不能为 null 或空白。", nameof(displayFontName));
        }

        DisplayFontName = displayFontName;
        FontFamily = fontFamily ?? throw new ArgumentNullException(nameof(fontFamily));
    }

    public override bool Equals(object obj)
    {
        return Equals(obj as FontInfo);
    }

    public bool Equals(FontInfo other)
    {
        return other is not null && FontFamily.Source == other.FontFamily.Source;
    }

    public override int GetHashCode()
    {
        int hashCode = 1329124491;
        hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(DisplayFontName);
        hashCode = hashCode * -1521134295 + EqualityComparer<FontFamily>.Default.GetHashCode(FontFamily);
        return hashCode;
    }

    public override string ToString() => DisplayFontName;

    public static bool operator ==(FontInfo left, FontInfo right)
    {
        return EqualityComparer<FontInfo>.Default.Equals(left, right);
    }

    public static bool operator !=(FontInfo left, FontInfo right)
    {
        return !(left == right);
    }
}
