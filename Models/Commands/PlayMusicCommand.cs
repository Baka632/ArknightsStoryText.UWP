namespace ArknightsResources.Stories.Models.Commands
{
    /// <summary>
    /// 表示播放音乐命令的类
    /// </summary>
    public class PlayMusicCommand : StoryCommand
    {
        /// <summary>
        /// 使用指定的参数初始化<seealso cref="PlayMusicCommand"/>类的新实例,该命令仅包括一个循环播放的音乐
        /// </summary>
        /// <param name="key">音乐代号</param>
        /// <param name="volume">音量</param>
        /// <param name="fadeTime">淡出时间</param>
        /// <param name="delay">延迟播放的时间</param>
        public PlayMusicCommand(string key, double volume, double fadeTime, double delay) : base(true)
        {
            Intro = key;
            Volume = volume;
            FadeTime = fadeTime;
            Delay = delay;
        }

        /// <summary>
        /// 使用指定的参数初始化<seealso cref="PlayMusicCommand"/>类的新实例,该命令包括一个引子音乐和循环音乐
        /// </summary>
        /// <param name="intro">引子音乐代号,该音乐将在开头播放</param>
        /// <param name="loop">循环音乐代号,在引子音乐播放完后将循环播放该音乐</param>
        /// <param name="volume">音量</param>
        /// <param name="fadeTime">淡出时间</param>
        /// <param name="delay">延迟播放的时间</param>
        public PlayMusicCommand(string intro, string loop, double volume, double fadeTime, double delay) : base(true)
        {
            Intro = intro;
            Loop = loop;
            Volume = volume;
            FadeTime = fadeTime;
            Delay = delay;
        }

        /// <summary>
        /// 引子音乐代号
        /// </summary>
        public string Intro { get; }

        /// <summary>
        /// 循环音乐代号
        /// </summary>
        public string Loop { get; }

        /// <summary>
        /// 音量
        /// </summary>
        public double Volume { get; }

        /// <summary>
        /// 淡出时间
        /// </summary>
        public double FadeTime { get; }

        /// <summary>
        /// 播放延迟时间
        /// </summary>
        public double Delay { get; set; }
    }
}
