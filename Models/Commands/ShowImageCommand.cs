using System;
using System.Collections.Generic;
using System.Text;

namespace ArknightsResources.Stories.Models.Commands
{
    public class ShowImageCommand : StoryCommand
    {
        //TODO: Add comment here

        public ShowImageCommand(string imageCodename, string screenAdaptMode, double fadeTime, bool isBlock,
                                bool isTiled, double xScale, double yScale, double x, double y) : base(true)
        {
            ImageCodename = imageCodename;
            ScreenAdaptMode = screenAdaptMode;
            FadeTime = fadeTime;
            IsBlock = isBlock;
            IsTiled = isTiled;
            XScale = xScale;
            YScale = yScale;
            X = x;
            Y = y;
        }


        public string ImageCodename { get; }
        
        public string ScreenAdaptMode { get; }
        
        public double FadeTime { get; }
        
        public bool IsBlock { get; }
        
        public bool IsTiled { get; }

        public double XScale { get; }

        public double YScale { get; }

        public double X { get; }
        
        public double Y { get; }
    }
}
