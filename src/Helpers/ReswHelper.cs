using Windows.ApplicationModel.Resources;

namespace ArknightsStoryText.UWP.Helpers
{
    internal class ReswHelper
    {
        public static string GetReswString(string name)
        {
            ResourceLoader loader = new();
            return loader.GetString(name);
        }
    }
}
