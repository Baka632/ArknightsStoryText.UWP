namespace ArknightsResources.Stories.Models.Commands
{
    public class ShowBlockerCommand : StoryCommand
    {
        //TODO:Add comment
        public ShowBlockerCommand(double a, double r, double g, double b, double fadeTime, bool isBlock) : base(true)
        {
            A = a;
            R = r;
            G = g;
            B = b;
            FadeTime = fadeTime;
            IsBlock = isBlock;
        }

        public double A { get; }

        public double R { get; }

        public double G { get; }

        public double B { get; }

        public double FadeTime { get; }

        public bool IsBlock { get; }
    }
}
