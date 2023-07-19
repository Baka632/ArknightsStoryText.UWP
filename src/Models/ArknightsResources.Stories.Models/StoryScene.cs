using ArknightsResources.Stories.Models.Commands;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace ArknightsResources.Stories.Models
{
    /// <summary>
    /// 表示一个剧情
    /// </summary>
    [DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "(),nq}")]
    public sealed class StoryScene : IEnumerable<StoryCommand>
    {
        internal StoryScene()
        {
            //For internal use
        }

        /// <summary>
        /// 使用给定的参数构造<see cref="StoryScene"/>的新实例
        /// </summary>
        /// <param name="storyCommands">包含剧情命令的数组</param>
        /// <param name="isSkippable">指示剧情是否可跳过</param>
        /// <param name="isAutoable">指示剧情是否可自动播放</param>
        /// <param name="fitMode">剧情的填充模式</param>
        /// <param name="comment">剧情文件的注释</param>
        public StoryScene(StoryCommand[] storyCommands,
                          bool isSkippable,
                          bool isAutoable,
                          string fitMode,
                          string comment = "")
        {
            StoryCommands = storyCommands;
            IsSkippable = isSkippable;
            IsAutoable = isAutoable;
            FitMode = fitMode;
            Comment = comment;
        }

        /// <summary>
        /// 一个剧情内所包含的命令
        /// </summary>
        public StoryCommand[] StoryCommands { get; internal set; }

        /// <summary>
        /// 指示剧情是否可跳过
        /// </summary>
        public bool IsSkippable { get; internal set; }

        /// <summary>
        /// 指示剧情是否可自动播放
        /// </summary>
        public bool IsAutoable { get; internal set; }

        /// <summary>
        /// 剧情的填充模式
        /// </summary>
        public string FitMode { get; internal set; }

        /// <summary>
        /// 剧情文件的注释
        /// </summary>
        public string Comment { get; internal set; }

        /// <inheritdoc/>
        public IEnumerator<StoryCommand> GetEnumerator()
        {
            return ((IEnumerable<StoryCommand>)StoryCommands).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return StoryCommands.GetEnumerator();
        }

        private string GetDebuggerDisplay()
        {
            return GetType().Name;
        }
    }
}
