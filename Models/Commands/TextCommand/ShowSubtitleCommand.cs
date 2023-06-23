namespace ArknightsResources.Stories.Models.Commands
{
    /// <summary>
    /// 表示一个显示剧情文本(字幕式)的命令
    /// </summary>
    public class ShowSubtitleCommand : TextCommand
    {
        //TODO:add comment
        public ShowSubtitleCommand(string text, double x, double y, string alignment, double size, double delay, double width) : base(text)
        {
            X = x;
            Y = y;
            Alignment = alignment;
            Size = size;
            Delay = delay;
            Width = width;
        }

        public double X { get; }

        public double Y { get; }

        public string Alignment { get; }

        public double Size { get; }

        public double Delay { get; }

        public double Width { get; }
    }
}
