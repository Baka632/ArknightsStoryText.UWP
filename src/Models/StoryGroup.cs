using System.Collections;

namespace ArknightsStoryText.UWP.Models;

public sealed class StoryGroup : IGrouping<ActType, StoryDataPack>
{
    private readonly IEnumerable<StoryDataPack> _packs;

    public ActType Key { get; }

    public StoryGroup(ActType key, IEnumerable<StoryDataPack> packs)
    {
        Key = key;
        _packs = packs;
    }

    public IEnumerator<StoryDataPack> GetEnumerator()
    {
        return _packs.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return _packs.GetEnumerator();
    }
}
