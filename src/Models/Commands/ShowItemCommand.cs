namespace ArknightsResources.Stories.Models.Commands
{
    public class ShowItemCommand : StoryCommand
    {
        public ShowItemCommand(string imageCodename, bool isBlock, double fadeTime, string fadeStyle, double offsetX) : base(true)
        {
            ImageCodename = imageCodename;
            IsBlock = isBlock;
            FadeTime = fadeTime;
            FadeStyle = fadeStyle;
            OffsetX = offsetX;
        }

        public string ImageCodename { get; }
        public bool IsBlock { get; }
        public double FadeTime { get; }
        public string FadeStyle { get; }
        public double OffsetX { get; }
    }
}