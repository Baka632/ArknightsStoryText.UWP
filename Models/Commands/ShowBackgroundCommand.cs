using System;
using System.Collections.Generic;
using System.Text;

namespace ArknightsResources.Stories.Models.Commands
{
    public class ShowBackgroundCommand : StoryCommand
    {
        //TODO:Add comment
        public ShowBackgroundCommand(string imageCodename, string screenAdaptMode, double fadeTime, double xScale,
                                     double yScale, double x, double y) : base(true)
        {
            ImageCodename = imageCodename;
            ScreenAdaptMode = screenAdaptMode;
            FadeTime = fadeTime;
            XScale = xScale;
            YScale = yScale;
            X = x;
            Y = y;
        }

        public string ImageCodename { get; }

        public string ScreenAdaptMode { get; }

        public double FadeTime { get; }

        public double XScale { get; }

        public double YScale { get; }

        public double X { get; }

        public double Y { get; }
    }
}
