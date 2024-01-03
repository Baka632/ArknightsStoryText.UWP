using ArknightsStoryText.UWP.Services;

namespace ArknightsStoryText.UWP.ViewModels;

public sealed class StoryGlanceViewModel : ObservableObject
{
    private readonly StoryMetadataService metadataService = new();

    private static void OpenGameDataFolder(StorageFolder gamedataFolder)
    {

    }
}
