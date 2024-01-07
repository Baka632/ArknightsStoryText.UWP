namespace ArknightsStoryText.UWP.Models;

public record StoryDataPack(string Name, IList<StoryInfo> Stories, StoryMetadataInfo StoryMetadata)
{
    public bool IsSingleStory { get; } = Stories.Count == 1;
}