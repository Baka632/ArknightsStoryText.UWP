namespace ArknightsResources.Stories.Models.Commands
{
    /// <summary>
    /// 表示一个展示Multiline文本的命令,Multiline可以分开展示一行文本中的内容
    /// </summary>
    public class ShowMultilineCommand : TextCommand
    {
        /// <summary>
        /// 使用给定的参数初始化<see cref="ShowMultilineCommand"/>类的新实例
        /// </summary>
        /// <param name="name">剧情人物名称</param>
        /// <param name="text">剧情文本</param>
        /// <param name="delay">播放延迟</param>
        /// <param name="end">指示是否为结束的值</param>
        public ShowMultilineCommand(string name, string text, double delay, bool end) : base(text)
        {
            Name= name;
            Delay = delay;
            IsEnd = end;
        }

        /// <summary>
        /// 剧情人物名称
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// 播放命令的延迟
        /// </summary>
        public double Delay { get; }

        /// <summary>
        /// 指示当前的<see cref="ShowMultilineCommand"/>是否为结束的值
        /// </summary>
        public bool IsEnd { get; }

        /// <inheritdoc/>
        protected override string GetDebuggerDisplay() => $"[{GetType().Name}] {Name}:{Text}";
    }
}
