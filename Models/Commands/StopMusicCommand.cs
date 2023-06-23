using System;
using System.Collections.Generic;
using System.Text;

namespace ArknightsResources.Stories.Models.Commands
{
    /// <summary>
    /// 表示停止当前音乐的命令
    /// </summary>
    public class StopMusicCommand : StoryCommand
    {
        /// <summary>
        /// 使用指定参数构造<seealso cref="StopMusicCommand"/>类的新实例
        /// </summary>
        /// <param name="fadeTime">音乐淡出时间</param>
        public StopMusicCommand(double fadeTime) : base(true)
        {
            FadeTime = fadeTime;
        }

        /// <summary>
        /// 音乐淡出时间
        /// </summary>
        public double FadeTime { get; }
    }
}
