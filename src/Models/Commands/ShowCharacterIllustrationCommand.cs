namespace ArknightsResources.Stories.Models.Commands
{
    public class ShowCharacterIllustrationCommand : StoryCommand
    {
        /// <summary>
        /// 使用给定的参数初始化<see cref="ShowCharacterIllustrationCommand"/>的新实例,该实例仅表示显示一张立绘的命令
        /// </summary>
        /// <param name="illustration">立绘图代号</param>
        /// <param name="fadeTime">立绘淡入时间</param>
        /// <param name="isBlock"></param>
        /// <param name="enterStyle">立绘进入效果</param>
        public ShowCharacterIllustrationCommand(string illustration, double fadeTime, bool isBlock,
                                                CharacterIllustrationEnterStyle enterStyle) : base(true)
        {
            IllustrationCodeName = illustration;
            FadeTime = fadeTime;
            IsBlock = isBlock;
            IllustrationEnterStyle = enterStyle;
            SecondIllustrationExists = false;
            Focus = 1;
        }

        /// <summary>
        /// 使用给定的参数初始化<see cref="ShowCharacterIllustrationCommand"/>的新实例,该实例表示显示两张立绘的命令
        /// </summary>
        /// <param name="illustration">第一张立绘图的代号</param>
        /// <param name="illustration2">第二张立绘图的代号</param>
        /// <param name="fadeTime">立绘淡入时间</param>
        /// <param name="isBlock"></param>
        /// <param name="focus">当前聚焦的立绘序号</param>
        /// <param name="enterStyle">第一张立绘的进入样式</param>
        /// <param name="enterStyle2">第二张立绘的进入样式</param>
        public ShowCharacterIllustrationCommand(string illustration, string illustration2, double fadeTime, bool isBlock,
                                                int focus, CharacterIllustrationEnterStyle enterStyle,
                                                CharacterIllustrationEnterStyle enterStyle2) : base(true)
        {
            IllustrationCodeName = illustration;
            IllustrationCodeName2 = illustration2;
            FadeTime = fadeTime;
            IsBlock = isBlock;
            Focus = focus;
            IllustrationEnterStyle = enterStyle;
            IllustrationEnterStyle2 = enterStyle2;
            SecondIllustrationExists = true;
        }

        /// <summary>
        /// 第一个立绘图的代号
        /// </summary>
        public string IllustrationCodeName { get; }
        
        /// <summary>
        /// 第二个立绘图的代号
        /// </summary>
        public string IllustrationCodeName2 { get; }

        /// <summary>
        /// 立绘淡出时间
        /// </summary>
        public double FadeTime { get; }

        public bool IsBlock { get; }

        /// <summary>
        /// 当前聚焦的立绘序号
        /// </summary>
        public int Focus { get; }

        /// <summary>
        /// 第一个立绘图的进入样式
        /// </summary>
        public CharacterIllustrationEnterStyle IllustrationEnterStyle { get; }

        /// <summary>
        /// 第二个立绘图的进入样式
        /// </summary>
        public CharacterIllustrationEnterStyle IllustrationEnterStyle2 { get; }

        /// <summary>
        /// 指示是否存在第二个立绘的值
        /// </summary>
        public bool SecondIllustrationExists { get; }
    }
}
