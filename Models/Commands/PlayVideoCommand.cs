using System;
using System.Collections.Generic;
using System.Text;

namespace ArknightsResources.Stories.Models.Commands
{
    /// <summary>
    /// 表示播放视频命令的类
    /// </summary>
    public class PlayVideoCommand : StoryCommand
    {
        /// <summary>
        /// 要播放的视频地址
        /// </summary>
        public string Source { get; }

        /// <summary>
        /// 使用指定的参数初始化<see cref="PlayVideoCommand"/>类的新实例
        /// </summary>
        /// <param name="source">视频源地址</param>
        public PlayVideoCommand(string source) : base(true)
        {
            Source = source;
        }
    }
}
