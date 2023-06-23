using System.Diagnostics;

namespace ArknightsResources.Stories.Models.Commands
{
    /// <summary>
    ///  剧情文件中文本命令的基类
    /// </summary>
    [DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "(),nq}")]
    public abstract class TextCommand : StoryCommand
    {
        /// <summary>
        /// 使用剧情文本初始化<seealso cref="TextCommand"/>类的新实例
        /// </summary>
        /// <param name="text">剧情文本</param>
        public TextCommand(string text) : base(false)
        {
            Text = text;
        }

        /// <summary>
        /// 剧情文本
        /// </summary>
        public string Text { get; }

        /// <inheritdoc/>
        protected override string GetDebuggerDisplay()
        {
            return Text;
        }
    }
}
