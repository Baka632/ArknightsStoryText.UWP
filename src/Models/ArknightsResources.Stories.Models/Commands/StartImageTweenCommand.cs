namespace ArknightsResources.Stories.Models.Commands
{
    public class StartImageTweenCommand : StoryCommand
    {
        public StartImageTweenCommand(string imageCodename, bool isBlock, double fadeTime, double duration,
                                      double xScaleFrom, double yScaleFrom, double xScaleTo, double yScaleTo,
                                      double xFrom, double yFrom, double xTo, double yTo) : base(true)
        {
            ImageCodename = imageCodename;
            IsBlock = isBlock;
            FadeTime = fadeTime;
            Duration = duration;
            XScaleFrom = xScaleFrom;
            YScaleFrom = yScaleFrom;
            XScaleTo = xScaleTo;
            YScaleTo = yScaleTo;
            XFrom = xFrom;
            YFrom = yFrom;
            XTo = xTo;
            YTo = yTo;
        }

        public string ImageCodename { get; }

        public bool IsBlock { get; }

        public double FadeTime { get; }
        
        public double Duration { get; }
        
        public double XScaleFrom { get; }
        
        public double YScaleFrom { get; }
        
        public double XScaleTo { get; }
        
        public double YScaleTo { get; }
        
        public double XFrom { get; }
        
        public double YFrom { get; }
        
        public double XTo { get; }
        
        public double YTo { get; }
    }
}