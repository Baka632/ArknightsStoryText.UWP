using System;
using System.Collections.Generic;
using System.Text;

namespace ArknightsResources.Stories.Models.Commands
{
    public class PlaySoundCommand : StoryCommand
    {
        //TODO:Add comment
        public PlaySoundCommand(string soundKey, string channel, double volume, double delay, bool isBlock, bool isLoop) : base(true)
        {
            SoundKey = soundKey;
            Channel = channel;
            Volume = volume;
            Delay = delay;
            IsBlock = isBlock;
            IsLoop = isLoop;
        }

        public string SoundKey { get; }

        public string Channel { get; }

        public double Volume { get; }

        public double Delay { get; }

        public bool IsBlock { get; }

        public bool IsLoop { get; }
    }
}
