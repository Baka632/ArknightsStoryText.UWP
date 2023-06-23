namespace ArknightsResources.Stories.Models.Commands
{
    /// <summary>
    /// 指示无操作的命令,该类一般只在代码内使用
    /// </summary>
    public class NoOperationCommand : StoryCommand
    {
        /// <summary>
        /// 初始化<see cref="NoOperationCommand"/>类的新实例
        /// </summary>
        public NoOperationCommand() : base(true)
        {
            //;)
        }
    }
}
