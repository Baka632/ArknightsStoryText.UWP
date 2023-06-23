using System.Diagnostics;

namespace ArknightsResources.Stories.Models.Commands
{
    /// <summary>
    /// 表示剧情文件的一行命令
    /// </summary>
    [DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "(),nq}")]
    public abstract class StoryCommand
    {
        /// <summary>
        /// 使用指定的参数初始化<see cref="StoryCommand"/>的新实例
        /// </summary>
        /// <param name="canAutoContinue">命令执行完后是否自动切换到下一个命令</param>
        public StoryCommand(bool canAutoContinue)
        {
            CanAutoContinue = canAutoContinue;
        }

        /// <summary>
        /// 指示命令执行完后是否自动切换到下一个命令
        /// </summary>
        public bool CanAutoContinue { get; }

        /// <summary>
        /// 为调试器提供控制该对象在调试器变量窗口中的显示方式的字符串
        /// </summary>
        /// <returns>该对象在调试器变量窗口中的显示方式的字符串</returns>
        protected virtual string GetDebuggerDisplay()
        {
            return GetType().Name;
        }
    }
}
