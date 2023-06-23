namespace ArknightsResources.Stories.Models.Commands
{
    /// <summary>
    /// 表示延迟下一条命令执行的命令
    /// </summary>
    public class DelayCommand : StoryCommand
    {
        /// <summary>
        /// 使用给定的参数初始化<see cref="DelayCommand"/>的新实例
        /// </summary>
        /// <param name="time">要延迟的时间</param>
        public DelayCommand(double time) : base(true)
        {
            Time = time;
        }

        /// <summary>
        /// 要延迟的时间
        /// </summary>
        public double Time { get; }
    }
}
