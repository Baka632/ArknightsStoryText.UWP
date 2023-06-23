namespace ArknightsResources.Stories.Models.Commands
{
    /// <summary>
    /// 表示一个隐藏对话框的命令
    /// </summary>
    public class HideDialogCommand : StoryCommand
    {
        /// <summary>
        /// 使用给定的参数构造<see cref="HideDialogCommand"/>类的新实例
        /// </summary>
        /// <param name="fadeTime">淡出时间</param>
        /// <param name="isBlock"></param>
        public HideDialogCommand(double fadeTime, bool isBlock) : base(true)
        {
            FadeTime = fadeTime;
            IsBlock = isBlock;
        }

        /// <summary>
        /// 淡出时间
        /// </summary>
        public double FadeTime { get; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsBlock { get; }
    }
}
