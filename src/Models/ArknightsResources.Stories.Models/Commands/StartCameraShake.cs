using System;
using System.Collections.Generic;
using System.Text;

namespace ArknightsResources.Stories.Models.Commands
{
    public class StartCameraShake : StoryCommand
    {
        public StartCameraShake(double duration, double xStrength, double yStrength, double vibrato, double randomness,
                                bool isFadeout, bool isBlock, bool isStop) : base(true)
        {
            Duration = duration;
            XStrength = xStrength;
            YStrength = yStrength;
            Vibrato = vibrato;
            Randomness = randomness;
            IsFadeout = isFadeout;
            IsBlock = isBlock;
            IsStop = isStop;
        }

        public double Duration { get; }

        public double XStrength { get; }
        
        public double YStrength { get; }
        
        public double Vibrato { get; }
        
        public double Randomness { get; }
        
        public bool IsFadeout { get; }
        
        public bool IsBlock { get; }
        
        public bool IsStop { get; }
    }
}
