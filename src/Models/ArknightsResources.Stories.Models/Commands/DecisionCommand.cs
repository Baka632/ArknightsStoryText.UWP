using System.Collections;
using System.Collections.Generic;

namespace ArknightsResources.Stories.Models.Commands
{
    /// <summary>
    /// 表示剧情内抉择的类
    /// </summary>
    public sealed class DecisionCommand : StoryCommand
    {
        /// <summary>
        /// 使用指定的参数初始化<seealso cref="DecisionCommand"/>类的新实例
        /// </summary>
        /// <param name="commands">剧情抉择所关联的一系列命令的数组</param>
        public DecisionCommand(Dictionary<string, StoryCommand[]> commands) : base(false)
        {
            PredicateSet = commands;

            string[] keys = new string[commands.Count];
            int i = 0;
            foreach (var item in commands)
            {
                keys[i] = item.Key;
                i++;
            }
            AvailableOptions = keys;
        }

        /// <summary>
        /// 通过指定的索引获取剧情抉择所关联的一系列命令
        /// </summary>
        /// <param name="index">指定的索引</param>
        /// <returns>某个剧情抉择所关联的一系列命令</returns>
        public StoryCommand[] this[string index]
        {
            get => PredicateSet[index];
        }

        /// <summary>
        /// 当前可用的选项
        /// </summary>
        public string[] AvailableOptions { get; }

        //键是选项(如“结果怎么样？”“......”“我的脑袋又热又胀，很不舒服。”)
        //值为各选项分支的命令(如”[name="凯尔希"]并没有什么新的进展。“)
        private Dictionary<string, StoryCommand[]> PredicateSet { get; }

        /// <inheritdoc/>
        protected override string GetDebuggerDisplay()
        {
            return nameof(DecisionCommand);
        }
    }
}
