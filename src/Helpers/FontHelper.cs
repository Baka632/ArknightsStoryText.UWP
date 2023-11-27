using System.Collections.Generic;
using System.Globalization;
using ArknightsStoryText.UWP.Models;

namespace ArknightsStoryText.UWP.Helpers;

public static class FontHelper
{
    private static IReadOnlyList<FontInfo> _fonts = null;

    public static IReadOnlyList<FontInfo> GetSystemFonts()
    {
        if (_fonts is not null)
        {
            return _fonts;
        }

        using SharpDX.DirectWrite.Factory factory = new();

        using SharpDX.DirectWrite.FontCollection fontCollection = factory.GetSystemFontCollection(false);

        int familyCount = fontCollection.FontFamilyCount;

        List<FontInfo> fonts = new(familyCount);

        for (int i = 0; i < familyCount; i++)
        {
            using SharpDX.DirectWrite.FontFamily fontFamily = fontCollection.GetFontFamily(i);
            using SharpDX.DirectWrite.LocalizedStrings familyNames = fontFamily.FamilyNames;

            string fontName = familyNames.FindLocaleName("en-us", out int invariantCultureNameIndex)
                ? familyNames.GetString(invariantCultureNameIndex)
                : familyNames.GetString(0);

            string displayName = familyNames.FindLocaleName(CultureInfo.CurrentUICulture.Name, out int localNameIndex)
                ? familyNames.GetString(localNameIndex)
                : fontName;

            fonts.Add(new FontInfo(displayName, new(fontName)));
        }

        _fonts = fonts;

        return fonts;
    }
}
