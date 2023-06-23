using System;
using System.Collections.Generic;
using System.Text;

namespace ArknightsResources.Stories.Models.Commands
{
    /// <summary>
    /// 表示隐藏剧情内干员立绘的命令
    /// </summary>
    public class HideCharacterIllustrationCommand : StoryCommand
    {
        /// <summary>
        /// 初始化<see cref="HideCharacterIllustrationCommand"/>类的新实例
        /// </summary>
        public HideCharacterIllustrationCommand() : base(true)
        {
            // :)
        }
    }
}
